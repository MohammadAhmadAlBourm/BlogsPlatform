using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal class BlogCategoryConfiguration : EntityConfiguration<BlogCategory>
{
    protected override void AppendConfig(EntityTypeBuilder<BlogCategory> builder)
    {
        builder.HasKey(e => e.Id).IsClustered();

        // Configure properties
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Slug)
            .IsRequired()
            .HasMaxLength(100);


        builder
            .HasMany(u => u.BlogPosts)
            .WithMany(p => p.Categories)
            .UsingEntity<Dictionary<string, object>>(
                "BlogPostCategory", // Name of the join table
                j => j
                    .HasOne<BlogPost>()
                    .WithMany()
                    .HasForeignKey("BlogPostId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<BlogCategory>()
                    .WithMany()
                    .HasForeignKey("BlogCategoryId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("BlogPostId", "BlogCategoryId");
                    j.ToTable("BlogPostCategories"); // Optional: customize join table name
                });

        builder.HasIndex(x => x.Slug).IsUnique();
    }
}