using BlogsPlatform.Abstractions;
using BlogsPlatform.Abstractions.Authentication;
using BlogsPlatform.Abstractions.Helpers;
using BlogsPlatform.Abstractions.Messaging;
using BlogsPlatform.Database;
using BlogsPlatform.Entities;
using BlogsPlatform.Extensions;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Features.BlogPosts;

public static class CreateBlogPost
{
    public sealed record Command(
        string Title,
        string Content,
        long? FeaturedImageId,
        List<long> CategoryIds,
        List<long> TagIds) : ICommand<Response>;

    public sealed class Response
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.");
        }
    }

    internal sealed class Handler : ICommandHandler<Command, Response>
    {
        private readonly BlogContext _context;
        private readonly IUserContext _userContext;
        public Handler(BlogContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<Result<Response>> Handle(Command command, CancellationToken cancellationToken)
        {
            long userId = _userContext.UserId;

            var author = await _context.Users.FindAsync([userId], cancellationToken);
            if (author is null)
            {
                return Result.Failure<Response>(Error.NotFound("User.NotFound", "Author not found."));
            }

            var slug = SlugGenerator.Create(command.Title);

            var blogPost = new BlogPost
            {
                Title = command.Title,
                Slug = slug,
                Content = command.Content,
                AuthorId = author.Id,
                FeaturedImageId = command.FeaturedImageId,
                IsPublished = false,
            };

            if (command.CategoryIds.Count != 0)
            {
                blogPost.Categories = await _context.BlogCategories
                    .Where(c => command.CategoryIds.Contains(c.Id))
                    .ToListAsync(cancellationToken);
            }

            if (command.TagIds.Count != 0)
            {
                blogPost.Tags = await _context.BlogTags
                    .Where(t => command.TagIds.Contains(t.Id))
                    .ToListAsync(cancellationToken);
            }

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync(cancellationToken);

            Response response = new()
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Slug = blogPost.Slug
            };

            return Result.Success(response);
        }
    }
}

public sealed class CreateBlogPostEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/blog-posts", async (CreateBlogPost.Command command, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.Ok(result.Value);
        })
        .WithName("CreateBlogPost")
        .Produces<CreateBlogPost.Response>(StatusCodes.Status201Created)
        .Produces<Error>(StatusCodes.Status400BadRequest)
        .RequireAuthorization();
    }
}