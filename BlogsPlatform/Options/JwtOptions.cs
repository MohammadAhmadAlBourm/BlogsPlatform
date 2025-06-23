using System.ComponentModel.DataAnnotations;

namespace BlogsPlatform.Options;

public class JwtOptions
{
    [Required]
    public required string Secret { get; init; }

    [Required]
    public int ExpirationInMinutes { get; init; }

    [Required]
    public required string Issuer { get; init; }
    [Required]
    public required string Audience { get; init; }
}