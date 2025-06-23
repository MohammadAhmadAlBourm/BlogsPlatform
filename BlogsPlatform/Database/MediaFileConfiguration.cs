using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal class MediaFileConfiguration : EntityConfiguration<MediaFile>
{
    protected override void AppendConfig(EntityTypeBuilder<MediaFile> builder)
    {
        builder.HasKey(e => e.Id).IsClustered();

        // Configure properties
        builder.Property(u => u.FileName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Url)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ContentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Size)
            .IsRequired();

        builder.Property(e => e.UploadedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();
    }
}