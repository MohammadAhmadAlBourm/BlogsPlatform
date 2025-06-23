using BlogsPlatform.Abstractions.Authentication;

namespace BlogsPlatform.Authentication;

internal sealed class UserContext(IHttpContextAccessor _httpContextAccessor) : IUserContext
{
    public bool IsAuthenticated => _httpContextAccessor
        .HttpContext?
        .User
        .Identity?
        .IsAuthenticated ?? false;

    public long UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new InvalidOperationException("User context is unavailable");
}