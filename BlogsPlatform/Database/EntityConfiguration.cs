using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogsPlatform.Database;

internal abstract class EntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : Entity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {

        AppendConfig(builder);
    }

    protected abstract void AppendConfig(EntityTypeBuilder<T> builder);
}