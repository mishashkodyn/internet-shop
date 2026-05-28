using Microsoft.AspNetCore.Identity;

namespace Shared.Infrastructure.Identity;

/// <summary>
/// Application user backed by ASP.NET Core Identity with a <see cref="Guid"/> primary key.
/// Lives in Shared.Infrastructure (not Shared.Domain) because it inherits from a framework/persistence
/// type — keeping it here preserves the persistence-ignorance of the Domain layer.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
}
