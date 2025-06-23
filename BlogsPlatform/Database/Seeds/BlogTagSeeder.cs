using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Database.Seeds;

public static class BlogTagSeeder
{

    public static async Task SeedAsync(BlogContext context)
    {
        foreach (var (name, slug) in DefaultTags)
        {
            var existingTag = await context.BlogTags
                .FirstOrDefaultAsync(t => t.Slug == slug);

            if (existingTag is null)
            {
                context.BlogTags.Add(new BlogTag
                {
                    Name = name,
                    Slug = slug
                });
            }
            else if (existingTag.Name != name) // Update if needed
            {
                existingTag.Name = name;
                context.BlogTags.Update(existingTag);
            }
        }

        await context.SaveChangesAsync();
    }

    private static readonly List<(string Name, string Slug)> DefaultTags = new()
    {
        ("Technology", "technology"),
        ("Programming", "programming"),
        ("DotNet", "dotnet"),
        ("CSharp", "csharp"),
        ("Web Development", "web-development"),
        ("Database", "database"),
        ("Cloud", "cloud"),
        ("AI", "ai"),
        ("Machine Learning", "machine-learning"),
        ("Security", "security"),
        ("DevOps", "devops"),
        ("Startups", "startups"),
        ("Productivity", "productivity"),
        ("Mobile", "mobile"),
        ("Flutter", "flutter"),
        ("React", "react"),
        ("Angular", "angular"),
        ("Design Patterns", "design-patterns"),
        ("Best Practices", "best-practices")
    };
}