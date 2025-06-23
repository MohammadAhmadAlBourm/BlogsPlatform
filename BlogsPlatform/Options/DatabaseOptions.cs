using System.ComponentModel.DataAnnotations;

namespace BlogsPlatform.Options;

public class DatabaseOptions
{
    [Required]
    public required string ConnectionString { get; set; }

    [Required]
    public int MaxRetryCount { get; init; }

    [Required]
    public int CommandTimeout { get; init; }

    [Required]
    public bool EnableDetailedErrors { get; init; }

    [Required]
    public bool EnableSensitiveDataLogging { get; init; }
}