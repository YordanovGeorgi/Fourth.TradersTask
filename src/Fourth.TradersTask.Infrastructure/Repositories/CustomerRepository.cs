using Fourth.TradersTask.Application.Abstractions;
using Fourth.TradersTask.Application.Models.Dtos;
using Fourth.TradersTask.Domain;
using Fourth.TradersTask.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fourth.TradersTask.Infrastructure.Repositories;

/// <summary>
/// Implementation of customer repository using Entity Framework Core.
/// </summary>
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(NorthwindDbContext dbContext) 
        : base(dbContext)
    {
    }

    /// <summary>
    /// Gets customers with pagination and optional search.
    /// </summary>
    public async Task<CustomerListDto> GetCustomersAsync(
        int pageNumber,
        int pageSize,
        string? customerName,
        CancellationToken cancellationToken = default)
    {
        var query = Query();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(customerName))
        {
            var term = customerName.Trim();
            query = query.Where(c => c.CompanyName.Contains(term));
        }

        // Get total count from filtered query
        var totalCount = await query.CountAsync(cancellationToken);

        // Calculate skip and apply pagination with includes
        var skip = (pageNumber - 1) * pageSize;
        var customers = await query
            .Include(c => c.Orders)
            .OrderBy(c => c.CompanyName)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new CustomerListDto
        {
            Customers = customers,
            TotalCount = totalCount
        };
    }

    /// <summary>
    /// Gets a customer with all related orders and order details.
    /// </summary>
    public async Task<Customer?> GetCustomerWithOrdersAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        return await Query()
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderDetails)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }
}
