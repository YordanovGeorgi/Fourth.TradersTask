using Fourth.TradersTask.Application.Abstractions;
using Fourth.TradersTask.Infrastructure.Data;
using Fourth.TradersTask.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fourth.TradersTask.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure DbContext: prefer configured connection string, fallback to in-memory to keep local/dev/tests runnable.
        var connectionString = configuration.GetConnectionString("NorthwindConnection")
            ?? throw new InvalidOperationException("Connection string 'NorthwindConnection' is not configured.");

        services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(connectionString));

        // Register open-generic repository
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        // Register unit of work using the DbContext
        services.AddScoped<IUnitOfWork, UnitOfWork>(sp =>
        {
            var dbContext = sp.GetRequiredService<NorthwindDbContext>();
            // UnitOfWork accepts DbContext; GenericRepository uses DbContext too
            return new UnitOfWork(dbContext);
        });

        // Keep existing concrete repository registration(s)
        // Register CustomerRepository (existing concrete repository) so existing services/tests that rely on ICustomerRepository remain valid.
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
