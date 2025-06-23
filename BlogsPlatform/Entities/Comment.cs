namespace BlogsPlatform.Entities;

public class Comment : Entity
{
    public long Id { get; set; }
    public long PostId { get; set; }
    public BlogPost Post { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsApproved { get; set; }

    public long? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; }
}