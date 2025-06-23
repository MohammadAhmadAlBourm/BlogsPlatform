using System.ComponentModel.DataAnnotations;

namespace BlogsPlatform.Options;

public class PasswordHasherOptions
{
    [Required]
    public int SaltSize { get; init; }
    [Required]
    public int HashSize { get; init; }

    [Required]
    public int Iterations { get; init; }
}