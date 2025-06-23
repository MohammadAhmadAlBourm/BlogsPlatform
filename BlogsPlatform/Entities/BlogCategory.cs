namespace BlogsPlatform.Entities;

public class BlogCategory : Entity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<BlogPost> BlogPosts { get; set; } = [];
}