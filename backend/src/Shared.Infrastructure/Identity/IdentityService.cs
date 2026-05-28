using Microsoft.AspNetCore.Identity;
using Shared.Application.Services;

namespace Shared.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<(bool Success, Guid UserId, IEnumerable<string> Errors)> CreateUserAsync(
        string email, string password, string displayName)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            DisplayName = displayName
        };
        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded
            ? (true, user.Id, [])
            : (false, Guid.Empty, result.Errors.Select(e => e.Description));
    }

    public async Task<UserRecord?> FindByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user is null ? null : new UserRecord(user.Id, user.Email!, user.DisplayName);
    }

    public async Task<UserRecord?> FindByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is null ? null : new UserRecord(user.Id, user.Email!, user.DisplayName);
    }

    public async Task<bool> CheckPasswordAsync(Guid userId, string password)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is not null && await userManager.CheckPasswordAsync(user, password);
    }

    public async Task RecordFailedLoginAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is not null) await userManager.AccessFailedAsync(user);
    }

    public async Task ResetFailedLoginCountAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is not null) await userManager.ResetAccessFailedCountAsync(user);
    }

    public async Task<bool> IsLockedOutAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is not null && await userManager.IsLockedOutAsync(user);
    }

    public async Task<IList<string>> GetRolesAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is null ? [] : await userManager.GetRolesAsync(user);
    }

    public async Task AddToRoleAsync(Guid userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is not null) await userManager.AddToRoleAsync(user, role);
    }
}
