namespace Shared.Application.Services;

public interface IIdentityService
{
    Task<(bool Success, Guid UserId, IEnumerable<string> Errors)> CreateUserAsync(
        string email, string password, string displayName);
    Task<UserRecord?> FindByEmailAsync(string email);
    Task<UserRecord?> FindByIdAsync(Guid userId);
    Task<bool> CheckPasswordAsync(Guid userId, string password);
    Task RecordFailedLoginAsync(Guid userId);
    Task ResetFailedLoginCountAsync(Guid userId);
    Task<bool> IsLockedOutAsync(Guid userId);
    Task<IList<string>> GetRolesAsync(Guid userId);
    Task AddToRoleAsync(Guid userId, string role);
}

public record UserRecord(Guid Id, string Email, string DisplayName);
