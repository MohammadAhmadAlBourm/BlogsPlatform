using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal class BlogPostConfiguration : EntityConfiguration<BlogPost>
{
    protected override void AppendConfig(EntityTypeBuilder<BlogPost> builder)
    {
        // Primary key
        builder.HasKey(x => x.Id).IsClustered();

        // Properties
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Slug)
            .IsUnique(); // Optional: enforce unique slugs

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.PublishedAt)
            .IsRequired();

        builder.Property(x => x.IsPublished)
            .IsRequired();

        // Relationships

        // Author (User) - Required
        builder.HasOne(x => x.Author)
            .WithMany(u => u.BlogPosts) // Ensure User has ICollection<BlogPost> AuthoredPosts
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Featured Image - Optional
        builder.HasOne(x => x.FeaturedImage)
            .WithMany() // Assuming MediaFile doesn't have navigation back to posts
            .HasForeignKey(x => x.FeaturedImageId)
            .OnDelete(DeleteBehavior.Restrict); // Set to null if image is deleted

        // Tags (Many-to-Many) — configured in BlogTagConfiguration

        // Categories (Many-to-Many) — should be configured in BlogCategoryConfiguration

        // Comments (One-to-Many)
        builder.HasMany(x => x.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        // Likes (One-to-Many)
        builder.HasMany(x => x.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}