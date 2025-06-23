using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal class CommentConfiguration : EntityConfiguration<Comment>
{
    protected override void AppendConfig(EntityTypeBuilder<Comment> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id).IsClustered();

        // Properties
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(1000); // Adjust as needed

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.Property(x => x.IsApproved)
            .IsRequired();

        // Relationship: Comment belongs to one BlogPost
        builder.HasOne(x => x.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Comment belongs to one User
        builder.HasOne(x => x.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Self-referencing relationship: ParentComment -> Replies
        builder.HasOne(x => x.ParentComment)
            .WithMany(x => x.Replies)
            .HasForeignKey(x => x.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cascading deletes on replies
    }
}