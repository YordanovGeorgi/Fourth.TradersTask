namespace Fourth.TradersTask.Application.Helpers;

/// <summary>
/// helper class for order-related calculations and operations.
/// </summary>
public static class OrderHelper
{
    /// <summary>
    /// Calculates the total value of an order from its order details.
    /// </summary>
    public static decimal CalculateOrderTotal(Domain.Order order)
    {
        return order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
    }
}
