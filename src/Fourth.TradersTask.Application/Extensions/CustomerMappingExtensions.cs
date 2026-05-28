using Fourth.TradersTask.Application.Helpers;
using Fourth.TradersTask.Application.Models.Dtos;
using Fourth.TradersTask.Domain;

namespace Fourth.TradersTask.Application.Extensions;

/// <summary>
/// Extension methods for mapping Customer domain entities to CustomerDetailDto.
/// </summary>
public static class CustomerMappingExtensions
{
    /// <summary>
    /// Creates a CustomerDetailDto populated with customer information and a list of order summaries sorted by order
    /// date descending.
    /// </summary>
    public static CustomerDetailDto ToCustomerDetailDto(this Customer customer)
    {
        return new CustomerDetailDto
        {
            CustomerId = customer.CustomerId,
            CompanyName = customer.CompanyName,
            ContactName = customer.ContactName,
            ContactTitle = customer.ContactTitle,
            Address = customer.Address,
            City = customer.City,
            Region = customer.Region,
            PostalCode = customer.PostalCode,
            Country = customer.Country,
            Phone = customer.Phone,
            Fax = customer.Fax,
            Orders = customer.Orders
                .OrderByDescending(o => o.OrderDate)
                .Select(order => new OrderSummaryDto
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalOrderValue = OrderHelper.CalculateOrderTotal(order),
                    NumberOfProducts = order.OrderDetails.Count
                })
                .ToList()
        };
    }
}