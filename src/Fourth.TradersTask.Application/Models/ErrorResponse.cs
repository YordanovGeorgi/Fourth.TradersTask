namespace Fourth.TradersTask.Application.Models;

/// <summary>
/// Unified error response model for API responses.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed error information.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Gets or sets the error code (optional).
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a new instance of ErrorResponse.
    /// </summary>
    public ErrorResponse() { }

    /// <summary>
    /// Creates a new instance of ErrorResponse with message.
    /// </summary>
    public ErrorResponse(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Creates a new instance of ErrorResponse with message and details.
    /// </summary>
    public ErrorResponse(string message, string? details = null)
    {
        Message = message;
        Details = details;
    }

    /// <summary>
    /// Creates a new instance of ErrorResponse with all parameters.
    /// </summary>
    public ErrorResponse(string message, string? details, string? errorCode)
    {
        Message = message;
        Details = details;
        ErrorCode = errorCode;
    }
}
