using System.Net;
using System.Text.Json;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Models;

namespace EmployeeManagement.API.Middleware;

/// <summary>
/// Global exception handling middleware that catches all unhandled exceptions
/// and returns standardized API responses. Never exposes raw stack traces.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Initializes the exception middleware.
    /// </summary>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processes the HTTP request and catches any unhandled exceptions.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Maps exceptions to appropriate HTTP status codes and response bodies.
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            Shared.Exceptions.ValidationException validationEx =>
                (HttpStatusCode.BadRequest,
                 ApiResponse<object>.FailureResponse("Validation failed", validationEx.Errors)),

            NotFoundException notFoundEx =>
                (HttpStatusCode.NotFound,
                 ApiResponse<object>.FailureResponse(notFoundEx.Message)),

            DuplicateException duplicateEx =>
                (HttpStatusCode.Conflict,
                 ApiResponse<object>.FailureResponse(duplicateEx.Message)),

            ConcurrencyException concurrencyEx =>
                (HttpStatusCode.Conflict,
                 ApiResponse<object>.FailureResponse(concurrencyEx.Message)),

            AuthenticationFailedException authEx =>
                (HttpStatusCode.Unauthorized,
                 ApiResponse<object>.FailureResponse(authEx.Message)),

            ForbiddenException forbiddenEx =>
                (HttpStatusCode.Forbidden,
                 ApiResponse<object>.FailureResponse(forbiddenEx.Message)),

            _ => HandleUnknownException(exception)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }

    /// <summary>
    /// Handles unexpected exceptions, logging details without exposing them.
    /// </summary>
    private (HttpStatusCode, ApiResponse<object>) HandleUnknownException(Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        return (HttpStatusCode.InternalServerError,
                ApiResponse<object>.FailureResponse("An internal server error occurred. Please try again later."));
    }
}
