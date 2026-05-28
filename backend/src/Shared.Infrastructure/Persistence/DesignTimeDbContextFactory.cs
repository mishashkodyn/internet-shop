using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shared.Infrastructure.Persistence;

/// <summary>
/// Lets the <c>dotnet ef</c> tooling construct <see cref="AppDbContext"/> at design time
/// without booting the WebHost. Used only by EF tooling — never at runtime.
/// The connection string is the LocalDB dev string; design-time operations don't
/// touch the database unless you explicitly run <c>database update</c>.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string DesignTimeConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=InternetShopDev;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";

    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(DesignTimeConnectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}
