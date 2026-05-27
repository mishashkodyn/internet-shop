using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

/// <summary>
/// Registers the Blog application layer: MediatR handlers and FluentValidation
/// validators discovered in this assembly.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddBlogApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
