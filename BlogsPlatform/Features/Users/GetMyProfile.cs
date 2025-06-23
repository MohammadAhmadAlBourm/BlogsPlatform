using BlogsPlatform.Abstractions;
using BlogsPlatform.Abstractions.Authentication;
using BlogsPlatform.Abstractions.Messaging;
using BlogsPlatform.Database;
using BlogsPlatform.Extensions;
using Carter;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Features.Users;

public static class GetMyProfile
{
    public sealed record Query() : IQuery<Response>;

    public sealed class Response
    {
        public long Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedDate { get; init; }
    }

    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        private readonly BlogContext _context;
        private readonly IUserContext _userContext;
        public Handler(BlogContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }
        public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            long userId = _userContext.UserId;

            var user = await _context.Users
                .Where(x => x.Id == userId)
                .Select(item => new Response
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    CreatedDate = item.CreatedDate
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return Result.Failure<Response>(Error.NotFound("NotFound", "User not found."));
            }

            return user;
        }
    }
}

public sealed class GetMyProfileEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/me", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetMyProfile.Query();
            var result = await sender.Send(query, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        })
        .WithName("GetMyProfile")
        .Produces<GetMyProfile.Response>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}