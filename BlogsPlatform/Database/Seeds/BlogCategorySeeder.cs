using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Database.Seeds;

public static class BlogCategorySeeder
{
    private static readonly List<(string Name, string Slug)> DefaultCategories = new()
    {
        ("Technology", "technology"),
        ("Programming", "programming"),
        ("Lifestyle", "lifestyle"),
        ("Business", "business"),
        ("Health & Fitness", "health-fitness"),
        ("Travel", "travel"),
        ("Food", "food"),
        ("Education", "education"),
        ("Finance", "finance"),
        ("Entertainment", "entertainment"),
        ("Sports", "sports"),
        ("News", "news"),
        ("Science", "science"),
        ("Culture", "culture"),
        ("Photography", "photography"),
        ("DIY", "diy"),
        ("Marketing", "marketing"),
        ("Startups", "startups"),
        ("Career", "career"),
        ("Personal Development", "personal-development")
    };

    public static async Task SeedAsync(BlogContext context)
    {
        foreach (var (name, slug) in DefaultCategories)
        {
            var existingCategory = await context.BlogCategories
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (existingCategory is null)
            {
                context.BlogCategories.Add(new BlogCategory
                {
                    Name = name,
                    Slug = slug
                });
            }
            else if (existingCategory.Name != name) // Update if needed
            {
                existingCategory.Name = name;
                context.BlogCategories.Update(existingCategory);
            }
        }

        await context.SaveChangesAsync();
    }
}