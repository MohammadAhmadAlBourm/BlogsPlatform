namespace BlogsPlatform.Entities;

public class User : Entity
{
    public long Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
    public ICollection<BlogPost> BlogPosts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<BlogLike> Likes { get; set; } = [];
}