using BlogsPlatform.Entities;

namespace BlogsPlatform.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
}