using BlogsPlatform.Abstractions;
using BlogsPlatform.Abstractions.Authentication;
using BlogsPlatform.Abstractions.Messaging;
using BlogsPlatform.Database;
using BlogsPlatform.Extensions;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Features.Users;

public static class Login
{
    public sealed record Command(
        string Email,
        string Password) : ICommand<LoginResponse>;

    public sealed class LoginResponse
    {
        public string Token { get; init; } = string.Empty;
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

    internal sealed class Handler : ICommandHandler<Command, LoginResponse>
    {
        private readonly BlogContext _context;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordHasher _passwordHasher;
        public Handler(BlogContext context, ITokenProvider tokenProvider, IPasswordHasher passwordHasher)
        {
            _context = context;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<LoginResponse>> Handle(Command command, CancellationToken cancellationToken)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

            if (user is null)
            {
                return Result.Failure<LoginResponse>(Error.NotFound("NotFound", "Invalid email or password."));
            }

            bool isVerified = _passwordHasher.Verify(command.Password, user.Password);
            if (!isVerified)
            {
                return Result.Failure<LoginResponse>(Error.NotFound("NotFound", "Invalid email or password."));
            }

            string token = _tokenProvider.Create(user);

            return new LoginResponse
            {
                Token = token
            };
        }
    }
}

public sealed class LoginEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/login", async (ISender sender, Login.Command command, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }
            return Results.Ok(result.Value);
        });
    }
}