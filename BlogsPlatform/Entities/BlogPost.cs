namespace BlogsPlatform.Entities;

public class BlogPost : Entity
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long AuthorId { get; set; }
    public User Author { get; set; }

    public long? FeaturedImageId { get; set; }
    public MediaFile FeaturedImage { get; set; }

    public DateTime PublishedAt { get; set; }
    public bool IsPublished { get; set; }

    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<BlogCategory> Categories { get; set; } = [];
    public ICollection<BlogTag> Tags { get; set; } = [];
    public ICollection<BlogLike> Likes { get; set; } = [];
}