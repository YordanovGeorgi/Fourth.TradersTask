namespace Fourth.TradersTask.Application.Helpers;

public static class OrderHelper
{
    public static decimal CalculateOrderTotal(Domain.Order order)
    {
        return order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
    }
}
