using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Identity;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure;

/// <summary>
/// Registers the Shared infrastructure layer: the EF Core <see cref="AppDbContext"/> and
/// ASP.NET Core Identity (keyed by <see cref="Guid"/>) with the application's password,
/// user, and lockout policies.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found in configuration.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // AddIdentityCore is the right surface for an API/token-based app hosted in a class
        // library — it avoids the cookie-auth wiring that AddIdentity pulls in (and the
        // FrameworkReference to Microsoft.AspNetCore.App that would imply).
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                // Password — min 8 chars, digit + non-alphanumeric required, no uppercase requirement.
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                // RequireLowercase left at its default (true).

                // User — emails must be unique.
                options.User.RequireUniqueEmail = true;

                // Lockout — 5 failed attempts, 5-minute lockout, applied to new users too.
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
