namespace Fourth.TradersTask.Application.Models;

/// <summary>
/// Response DTO for order summary information.
/// </summary>
public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalOrderValue { get; set; }
    public int NumberOfProducts { get; set; }
    public OrderSummaryDto() { }

    public OrderSummaryDto(int orderId, DateTime? orderDate, decimal totalOrderValue, int numberOfProducts)
    {
        OrderId = orderId;
        OrderDate = orderDate;
        TotalOrderValue = totalOrderValue;
        NumberOfProducts = numberOfProducts;
    }}
