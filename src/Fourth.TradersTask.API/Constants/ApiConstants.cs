namespace Fourth.TradersTask.API.Constants;

/// <summary>
/// API-wide constants and configuration values.
/// </summary>
public static class ApiConstants
{
    public const string ApiVersion = "v1";
    public const string ApiTitle = "Northwind Traders API";
    public const string ApiDescription = "Backend API for managing Northwind Traders customer information";
    
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;
    
    public const string CustomerName = "none";
    
    // Error messages
    public const string InvalidPageNumberMessage = "Page number must be greater than 0.";
    public const string InvalidPageSizeMessage = "Page size must be between 1 and 100.";
    public const string EmptyCustomerIdMessage = "Customer ID cannot be empty.";
    public const string CustomerNotFoundMessage = "Customer with ID '{0}' not found.";
    public const string ErrorOccurredMessage = "An error occurred while processing your request.";
}
