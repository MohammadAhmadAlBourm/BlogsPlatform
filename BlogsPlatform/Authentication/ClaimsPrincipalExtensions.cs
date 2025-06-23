using System.Security.Claims;

namespace BlogsPlatform.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);


        if (long.TryParse(userId, out long parsedUserId))
        {
            return parsedUserId;
        }
        else
        {
            throw new InvalidOperationException("User id is unavailable");
        }
    }
}