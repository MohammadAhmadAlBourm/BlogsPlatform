using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal class BlogTagConfiguration : EntityConfiguration<BlogTag>
{
    protected override void AppendConfig(EntityTypeBuilder<BlogTag> builder)
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
            .WithMany(p => p.Tags)
            .UsingEntity<Dictionary<string, object>>(
                "BlogPostTag", // Name of the join table
                j => j
                    .HasOne<BlogPost>()
                    .WithMany()
                    .HasForeignKey("BlogPostId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<BlogTag>()
                    .WithMany()
                    .HasForeignKey("BlogTagId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("BlogPostId", "BlogTagId");
                    j.ToTable("BlogPostTags"); // Optional: customize join table name
                });

        builder.HasIndex(x => x.Slug).IsUnique();
    }
}