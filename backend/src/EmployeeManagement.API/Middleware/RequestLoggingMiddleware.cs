namespace EmployeeManagement.API.Middleware;

/// <summary>
/// Middleware that logs incoming HTTP requests and their response status codes.
/// Sensitive data (passwords, tokens) is never logged.
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    /// <summary>
    /// Initializes the request logging middleware.
    /// </summary>
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Logs request method, path, and response status code.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(
            "HTTP {Method} {Path} started",
            context.Request.Method,
            context.Request.Path);

        await _next(context);

        _logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode);
    }
}
