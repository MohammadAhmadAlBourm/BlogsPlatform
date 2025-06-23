using BlogsPlatform.Abstractions.DomainEvents;
using BlogsPlatform.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogsPlatform.Database;

public class BlogContext(DbContextOptions<BlogContext> options, IDomainEventsDispatcher domainEventsDispatcher) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<BlogTag> BlogTags { get; set; }
    public DbSet<BlogLike> BlogLikes { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            }).ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}