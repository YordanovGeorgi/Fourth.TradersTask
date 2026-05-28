using System.Net;
using System.Text.Json;
using Fourth.TradersTask.Application.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fourth.TradersTask.API.Middleware;

/// <summary>
/// Global exception handling middleware with comprehensive exception type mapping.
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {ExceptionType} - {Message}", 
                ex.GetType().Name, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorCode, message, details) = MapException(exception);

        context.Response.StatusCode = statusCode;

        var response = new ErrorResponse(
            message,
            details,
            errorCode);

        return context.Response.WriteAsJsonAsync(response);
    }

    /// <summary>
    /// Maps exceptions to appropriate HTTP status codes and error messages.
    /// </summary>
    private static (int StatusCode, string ErrorCode, string Message, string? Details) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentException => (
                (int)HttpStatusCode.BadRequest,
                "INVALID_ARGUMENT",
                "One or more validation errors occurred.",
                exception.Message),

            KeyNotFoundException => (
                (int)HttpStatusCode.NotFound,
                "NOT_FOUND",
                "The requested resource was not found.",
                exception.Message),

            SqlException sqlEx => (
                sqlEx.Number switch
                {
                    -2 => ((int)HttpStatusCode.BadRequest, "DB_TIMEOUT", "Database connection timeout", null),
                    -1 => ((int)HttpStatusCode.ServiceUnavailable, "DB_CONNECTION_FAILED", "Database connection failed", null),
                    _ => ((int)HttpStatusCode.InternalServerError, "DB_ERROR", "A database error occurred", null)
                }),

            DbUpdateConcurrencyException => (
                (int)HttpStatusCode.Conflict,
                "CONCURRENCY_ERROR",
                "The resource has been modified by another user. Please refresh and try again.",
                null),

            DbUpdateException => (
                (int)HttpStatusCode.InternalServerError,
                "DB_UPDATE_ERROR",
                "A database update error occurred.",
                exception.InnerException?.Message),

            OperationCanceledException => (
                (int)HttpStatusCode.RequestTimeout,
                "OPERATION_CANCELLED",
                "The operation was cancelled.",
                null),

            NotImplementedException => (
                (int)HttpStatusCode.NotImplemented,
                "NOT_IMPLEMENTED",
                "This feature is not yet implemented.",
                exception.Message),

            UnauthorizedAccessException => (
                (int)HttpStatusCode.Forbidden,
                "UNAUTHORIZED",
                "You do not have permission to access this resource.",
                null),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                "INTERNAL_ERROR",
                "An unexpected error occurred while processing your request.",
                exception.Message)
        };
    }
}
