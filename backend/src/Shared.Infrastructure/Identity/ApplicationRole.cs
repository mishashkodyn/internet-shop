
using Microsoft.AspNetCore.Identity;

namespace Shared.Infrastructure.Identity;

/// <summary>
/// Application role backed by ASP.NET Core Identity with a <see cref="Guid"/> primary key.
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
}
