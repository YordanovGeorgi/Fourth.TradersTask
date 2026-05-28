using Fourth.TradersTask.Application.Abstractions;
using Fourth.TradersTask.Application.Models;
using Fourth.TradersTask.Application.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace Fourth.TradersTask.Application.Services;

/// <summary>
/// Implementation of the customer service.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    /// <summary>
    /// Constructor for CustomerService.
    /// </summary>
    /// <param name="customerRepository"></param>
    /// <param name="logger"></param>
    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets a paginated list of customers with optional search filtering.
    /// </summary>
    public async Task<PagedResult<CustomerListItemDto>> GetCustomersAsync(
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching customers with page {PageNumber}, size {PageSize}, search term: {SearchTerm}",
            paginationParams.PageNumber, paginationParams.PageSize, paginationParams.CustomerName ?? "none");

        var result = await _customerRepository.GetCustomersAsync(
            paginationParams.PageNumber,
            paginationParams.PageSize,
            paginationParams.CustomerName,
            cancellationToken);

        var dtos = result.Customers.Select(c => new CustomerListItemDto(
            c.CustomerId,
            c.CompanyName,
            c.Orders.Count)).ToList();

        _logger.LogInformation("Successfully fetched {Count} customers", dtos.Count);

        return new PagedResult<CustomerListItemDto>(
            paginationParams.PageNumber,
            paginationParams.PageSize,
            result.TotalCount,
            dtos);
    }

    /// <summary>
    /// Gets detailed information about a specific customer including order summaries.
    /// </summary>
    public async Task<CustomerDetailDto?> GetCustomerDetailAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching customer details for customer ID: {CustomerId}", customerId);

        var customer = await _customerRepository.GetCustomerWithOrdersAsync(customerId, cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer not found for customer ID: {CustomerId}", customerId);
            return null;
        }

        var customerDetailDto = new CustomerDetailDto
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
                .Select(o => new OrderSummaryDto(
                    o.OrderId,
                    o.OrderDate,
                    CalculateOrderTotal(o),
                    o.OrderDetails.Count))
                .ToList()
        };

        _logger.LogInformation("Successfully fetched customer details with {OrderCount} orders",
            customerDetailDto.Orders.Count);

        return customerDetailDto;
    }

    /// <summary>
    /// Calculates the total value of an order from its order details.
    /// </summary>
    private static decimal CalculateOrderTotal(Domain.Order order)
    {
        return order.OrderDetails.Sum(od => 
            od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
    }
}
