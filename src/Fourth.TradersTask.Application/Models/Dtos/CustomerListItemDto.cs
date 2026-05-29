namespace Fourth.TradersTask.Application.Models.Dtos;

/// <summary>
/// Response DTO for a customer in a list.
/// </summary>
public class CustomerListItemDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int NumberOfOrders { get; set; }

    public CustomerListItemDto() { }

    public CustomerListItemDto(string customerId, string customerName, int numberOfOrders)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        NumberOfOrders = numberOfOrders;
    }
}
