namespace EmployeeManagement.Shared.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of NotFoundException.
    /// </summary>
    public NotFoundException(string message) : base(message) { }

    /// <summary>
    /// Creates a NotFoundException for a specific entity type and ID.
    /// </summary>
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with ID '{key}' was not found.") { }
}

/// <summary>
/// Exception thrown when a concurrency conflict occurs.
/// </summary>
public class ConcurrencyException : Exception
{
    /// <summary>
    /// Initializes a new instance of ConcurrencyException.
    /// </summary>
    public ConcurrencyException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when a duplicate record is detected.
/// </summary>
public class DuplicateException : Exception
{
    /// <summary>
    /// Initializes a new instance of DuplicateException.
    /// </summary>
    public DuplicateException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when input validation fails.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Dictionary of field names to their validation error messages.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Initializes a new instance of ValidationException with error details.
    /// </summary>
    public ValidationException(IDictionary<string, string[]> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}

/// <summary>
/// Exception thrown when authentication fails.
/// </summary>
public class AuthenticationFailedException : Exception
{
    /// <summary>
    /// Initializes a new instance of AuthenticationFailedException.
    /// </summary>
    public AuthenticationFailedException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when authorization fails.
/// </summary>
public class ForbiddenException : Exception
{
    /// <summary>
    /// Initializes a new instance of ForbiddenException.
    /// </summary>
    public ForbiddenException(string message = "You do not have permission to perform this action.") : base(message) { }
}
