using Fourth.TradersTask.Application.Models.Dtos;
using Fourth.TradersTask.Domain;

namespace Fourth.TradersTask.Application.Abstractions;

/// <summary>
/// Abstraction for data access operations.
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// Gets customers with pagination and search support.
    /// </summary>
    Task<CustomerListDto> GetCustomersAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a customer by ID with all related orders and order details.
    /// </summary>
    Task<Customer?> GetCustomerWithOrdersAsync(string customerId, CancellationToken cancellationToken = default);
}
