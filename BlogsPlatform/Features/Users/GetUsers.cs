using BlogsPlatform.Abstractions;
using BlogsPlatform.Abstractions.Messaging;
using BlogsPlatform.Database;
using BlogsPlatform.Extensions;
using Carter;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Features.Users;

public static class GetUsers
{
    public sealed record Query() : IQuery<List<Response>>;

    public sealed class Response
    {
        public long Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedDate { get; init; }
    }

    internal sealed class Handler : IQueryHandler<Query, List<Response>>
    {
        private readonly BlogContext _context;

        public Handler(BlogContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Response>>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Select(item => new Response
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    CreatedDate = item.CreatedDate
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}

public sealed class GetUsersEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUsers.Query();
            var result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            return Results.Ok(result.Value);
        }).RequireAuthorization();
    }
}