using FluentAssertions;
using Fourth.TradersTask.Application.Helpers;
using Xunit;

namespace Fourth.TradersTask.UnitTests;

public class OrderHelperTests
{
    [Fact]
    public void CalculateOrderTotal_CalculatesCorrectTotal()
    {
        // Act
        var order = new Domain.Order()
        {
            OrderDetails = new List<Domain.OrderDetail>()
            {
                new Domain.OrderDetail()
                {
                    ProductId = 1,
                    Quantity = 10,
                    UnitPrice = 20.5m
                }
            }
        };

        var result = OrderHelper.CalculateOrderTotal(order);

        // Assert
        result.Should().Be(205m);
    }

    [Fact]
    public void CalculateOrderTotal_CalculatesCorrectTotal_WithDiscount()
    {
        // Act
        var order = new Domain.Order()
        {
            OrderDetails = new List<Domain.OrderDetail>()
            {
                new Domain.OrderDetail()
                {
                    ProductId = 1,
                    Quantity = 10,
                    UnitPrice = 20.5m,
                    Discount = 0.1f
                }
            }
        };

        var result = OrderHelper.CalculateOrderTotal(order);

        // Assert
        result.Should().Be(184.5m);
    }
}
