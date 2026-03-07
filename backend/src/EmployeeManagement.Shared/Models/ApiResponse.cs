namespace EmployeeManagement.Shared.Models;

/// <summary>
/// Standard API response wrapper used by all endpoints.
/// </summary>
/// <typeparam name="T">The type of data being returned.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Human-readable message describing the result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The data payload, null on failure.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Validation or error details, null on success.
    /// </summary>
    public object? Errors { get; set; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    /// <summary>
    /// Creates a failure response with error details.
    /// </summary>
    public static ApiResponse<T> FailureResponse(string message, object? errors = null)
    {
        return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}
