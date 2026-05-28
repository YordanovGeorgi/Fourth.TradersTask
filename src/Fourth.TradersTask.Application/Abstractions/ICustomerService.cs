using Fourth.TradersTask.Application.Models;
using Fourth.TradersTask.Application.Models.Dtos;

namespace Fourth.TradersTask.Application.Abstractions;

/// <summary>
/// Service interface for customer operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Gets a paginated list of customers with optional filtering.
    /// </summary>
    Task<PagedResult<CustomerListItemDto>> GetCustomersAsync(
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets detailed information about a specific customer including order summaries.
    /// </summary>
    Task<CustomerDetailDto?> GetCustomerDetailAsync(
        string customerId,
        CancellationToken cancellationToken = default);
}
