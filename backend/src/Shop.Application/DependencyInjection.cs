using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Application;

/// <summary>
/// Registers the Shop application layer: MediatR handlers and FluentValidation
/// validators discovered in this assembly.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddShopApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
