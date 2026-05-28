namespace Fourth.TradersTask.Application.Models;

/// <summary>
/// Response DTO for detailed customer information.
/// </summary>
public class CustomerDetailDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public List<OrderSummaryDto> Orders { get; set; } = new();

    public CustomerDetailDto() { }
}
