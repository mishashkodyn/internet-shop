namespace Shared.Application.Auth;

public record TokenPairDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
