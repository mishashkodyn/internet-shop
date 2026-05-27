using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure;

/// <summary>
/// Registers the Shared infrastructure layer (shared persistence, Identity).
/// Placeholder for now — DbContext / Identity wiring is added in a later step.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: register the shared DbContext (connection string from
        // configuration.GetConnectionString("DefaultConnection")) and Identity here.
        return services;
    }
}
