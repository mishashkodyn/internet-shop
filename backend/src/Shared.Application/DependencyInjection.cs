using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviours;

namespace Shared.Application;

/// <summary>
/// Registers the Shared application layer: MediatR handlers and FluentValidation
/// validators discovered in this assembly.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddSharedApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
