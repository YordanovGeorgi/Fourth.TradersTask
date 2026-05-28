using Fourth.TradersTask.Application.Abstractions;
using Fourth.TradersTask.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fourth.TradersTask.Infrastructure.Repositories;

/// <summary>
/// Implementation of customer repository using Entity Framework Core.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly Data.NorthwindDbContext _dbContext;

    public CustomerRepository(Data.NorthwindDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets customers with pagination and optional search.
    /// </summary>
    public async Task<(List<Customer> Items, int TotalCount)> GetCustomersAsync(
        int pageNumber,
        int pageSize,
        string? customerName,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Customers.AsQueryable();

        // Apply search filter
        query = AddSearchByCustomerName(customerName, query);

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

        return (customers, totalCount);
    }

    /// <summary>
    /// Gets a customer with all related orders and order details.
    /// </summary>
    public async Task<Customer?> GetCustomerWithOrdersAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderDetails)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
    }

    private static IQueryable<Customer> AddSearchByCustomerName(string? customerName, IQueryable<Customer> query)
    {
        if (!string.IsNullOrWhiteSpace(customerName))
        {
            var term = customerName.Trim();
            query = query.Where(c => c.CompanyName.Contains(term));
        }

        return query;
    }
}
