using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Infrastructure;

/// <summary>
/// Registers the Shop infrastructure layer (persistence, external services).
/// Placeholder for now — the Shop DbContext is added in a later step.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddShopInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: register the Shop DbContext (connection string from
        // configuration.GetConnectionString("DefaultConnection")) here.
        return services;
    }
}
