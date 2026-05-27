using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

/// <summary>
/// Registers the Blog infrastructure layer (persistence, external services).
/// Placeholder for now — the Blog DbContext is added in a later step.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddBlogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: register the Blog DbContext (connection string from
        // configuration.GetConnectionString("DefaultConnection")) here.
        return services;
    }
}
