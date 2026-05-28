namespace Shared.Application.Auth;

public record UserInfoDto(Guid UserId, string Email, string DisplayName, IReadOnlyList<string> Roles);
