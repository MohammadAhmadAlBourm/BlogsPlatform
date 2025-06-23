using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal class BlogLikeConfiguration : EntityConfiguration<BlogLike>
{
    protected override void AppendConfig(EntityTypeBuilder<BlogLike> builder)
    {
        builder.HasKey(x => x.Id).IsClustered();

        builder.Property(x => x.LikedDate)
            .IsRequired();

        // Relation with BlogPost
        builder.HasOne(x => x.Post)
            .WithMany(p => p.Likes) // Ensure BlogPost has ICollection<BlogLike> Likes
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict); // Optional, adjust based on your cascade needs

        // Relation with User
        builder.HasOne(x => x.User)
            .WithMany(u => u.Likes) // Ensure User has ICollection<BlogLike> LikedPosts
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Optional

        // Optional: Prevent duplicate likes
        builder.HasIndex(x => new { x.PostId, x.UserId }).IsUnique();
    }
}