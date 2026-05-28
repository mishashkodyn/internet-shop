namespace Shared.Application.Services;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles, string displayName);
    string GenerateRefreshToken();
}
