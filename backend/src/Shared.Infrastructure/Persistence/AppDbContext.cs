using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Identity;

namespace Shared.Infrastructure.Persistence;

/// <summary>
/// The shared EF Core DbContext. Inherits ASP.NET Core Identity's <see cref="IdentityDbContext{TUser,TRole,TKey}"/>
/// so the standard Identity tables (AspNetUsers, AspNetRoles, AspNetUserClaims, AspNetUserLogins,
/// AspNetUserRoles, AspNetUserTokens, AspNetRoleClaims) are part of the schema, keyed by <see cref="Guid"/>.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Shop/Blog entity configurations will be applied here in later steps, e.g.:
        // builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
