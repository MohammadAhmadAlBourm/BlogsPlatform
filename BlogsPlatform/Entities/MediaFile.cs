namespace BlogsPlatform.Entities;

public class MediaFile : Entity
{
    public long Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime UploadedDate { get; set; }
}