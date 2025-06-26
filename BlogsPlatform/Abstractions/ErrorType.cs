namespace BlogsPlatform.Abstractions;

public enum ErrorType
{
    Failure = 0,            // General failure
    Validation = 1,         // Input validation failed
    Problem = 2,            // Unexpected runtime problem
    NotFound = 3,           // Resource not found
    Conflict = 4,           // Conflict with current state (e.g., duplicate)

    Unauthorized = 5,       // Authentication required or failed
    Forbidden = 6,          // Authenticated, but not allowed
    BadRequest = 7,         // Generic bad input
    Timeout = 8,            // Operation took too long
    DependencyFailure = 9,  // External system failed (e.g., database, API)
    Unavailable = 10,       // Service temporarily unavailable
    RateLimited = 11,       // Too many requests
    UnsupportedMediaType = 12, // Content-Type not supported
    Internal = 13,          // Internal server error
    UnprocessableEntity = 14, // Semantic validation failed
    AlreadyExists = 15,     // Resource already exists
    Canceled = 16,          // Operation was canceled
    InvalidState = 17       // Operation not allowed in current state
}