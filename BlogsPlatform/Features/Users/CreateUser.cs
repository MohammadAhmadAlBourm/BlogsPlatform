using BlogsPlatform.Abstractions;
using BlogsPlatform.Abstractions.Authentication;
using BlogsPlatform.Abstractions.Messaging;
using BlogsPlatform.Database;
using BlogsPlatform.Entities;
using BlogsPlatform.Extensions;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Features.Users;

public static class CreateUser
{
    public sealed record Command(
        string FirstName,
        string LastName,
        string Email) : ICommand<CreateUserResponse>;

    public sealed class CreateUserResponse
    {
        public long Id { get; set; }
    }

    internal sealed class UserValidator : AbstractValidator<Command>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
        }
    }

    internal sealed class Handler : ICommandHandler<Command, CreateUserResponse>
    {
        private readonly BlogContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordGenerator _passwordGenerator;
        public Handler(BlogContext context, IPasswordHasher passwordHasher, IPasswordGenerator passwordGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _passwordGenerator = passwordGenerator;
        }

        public async Task<Result<CreateUserResponse>> Handle(Command command, CancellationToken cancellationToken)
        {
            User user = new()
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Username = command.Email.Split('@')[0],
                Email = command.Email,
                CreatedDate = DateTime.Now,
            };

            string password = _passwordGenerator.Generate();
            user.Password = _passwordHasher.Hash(password);

            bool isExist = await _context.Users.AnyAsync(x => x.Username == user.Username, cancellationToken);
            if (isExist)
            {
                return Result.Failure<CreateUserResponse>(
                    Error.Validation(
                        "User.Exist",
                        "A user with this username already exists. Please choose a different username."));
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            // Optionally, you can send an email to the user with their credentials


            CreateUserResponse response = new()
            {
                Id = user.Id
            };

            return Result.Success(response);
        }
    }
}

public sealed class CreateUserEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/create-user", async (CreateUser.Command command, ISender sender, CancellationToken cancellationToken) =>
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