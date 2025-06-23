namespace BlogsPlatform.Abstractions.Authentication;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    long UserId { get; }
}