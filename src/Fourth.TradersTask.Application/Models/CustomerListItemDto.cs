namespace Fourth.TradersTask.Application.Models;

/// <summary>
/// Response DTO for a customer in a list.
/// </summary>
public class CustomerListItemDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public int NumberOfOrders { get; set; }
}
