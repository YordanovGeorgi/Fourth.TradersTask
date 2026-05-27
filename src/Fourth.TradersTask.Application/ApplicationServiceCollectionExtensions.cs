using Fourth.TradersTask.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fourth.TradersTask.Application;

/// <summary>
/// Extension methods for registering application services.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}
