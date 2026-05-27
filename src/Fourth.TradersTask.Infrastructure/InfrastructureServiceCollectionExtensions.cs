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
        var connectionString = configuration.GetConnectionString("NorthwindConnection")
            ?? throw new InvalidOperationException("Connection string 'NorthwindConnection' is not configured.");

        services.AddDbContext<NorthwindDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
