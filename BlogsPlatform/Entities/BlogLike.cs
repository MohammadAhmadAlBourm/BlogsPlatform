namespace BlogsPlatform.Entities;

public class BlogLike : Entity
{
    public long Id { get; set; }
    public long PostId { get; set; }
    public BlogPost Post { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public DateTime LikedDate { get; set; }
}