using FluentValidation;

namespace BlogsPlatform.Features.BlogCategories;

public static class CreateBlogCategory
{
    public record Command(string Name, string Description);
    public record Response(int Id, string Name, string Description);
    internal sealed class Validator : AbstractValidator<Command>
    {

    }
    internal static class Handler
    {
        public static Response Handle(Command command)
        {
            // Simulate creating a blog category and returning the response
            return new Response(1, command.Name, command.Description);
        }
    }



}
