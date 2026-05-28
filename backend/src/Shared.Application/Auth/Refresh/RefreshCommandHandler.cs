using MediatR;
using Shared.Application.Abstractions;
using Shared.Application.Auth;
using Shared.Application.Services;
using Shared.Domain.Identity;

namespace Shared.Application.Auth.Refresh;

public class RefreshCommandHandler(
    IIdentityService identityService,
    IRefreshTokenRepository tokenRepo,
    IJwtService jwtService) : IRequestHandler<RefreshCommand, AuthResult<TokenPairDto>>
{
    public async Task<AuthResult<TokenPairDto>> Handle(RefreshCommand cmd, CancellationToken ct)
    {
        var existing = await tokenRepo.FindByTokenAsync(cmd.RefreshToken, ct);
        if (existing is null)
            return AuthResult<TokenPairDto>.Fail("INVALID_TOKEN", "Refresh token not found.");

        if (existing.RevokedAt is not null)
        {
            // Token reuse detected — revoke entire family to mitigate theft
            await tokenRepo.RevokeAllActiveForUserAsync(existing.UserId, ct);
            await tokenRepo.SaveChangesAsync(ct);
            return AuthResult<TokenPairDto>.Fail("TOKEN_REUSE", "Refresh token has already been used.");
        }

        if (existing.ExpiresAt < DateTime.UtcNow)
        {
            existing.RevokedAt = DateTime.UtcNow;
            await tokenRepo.SaveChangesAsync(ct);
            return AuthResult<TokenPairDto>.Fail("TOKEN_EXPIRED", "Refresh token has expired.");
        }

        var user = await identityService.FindByIdAsync(existing.UserId);
        if (user is null || await identityService.IsLockedOutAsync(existing.UserId))
        {
            existing.RevokedAt = DateTime.UtcNow;
            await tokenRepo.SaveChangesAsync(ct);
            return AuthResult<TokenPairDto>.Fail("INVALID_TOKEN", "User not found or locked.");
        }

        // Rotate: revoke old, issue new
        var newRefreshTokenString = jwtService.GenerateRefreshToken();
        existing.RevokedAt = DateTime.UtcNow;
        existing.ReplacedByToken = newRefreshTokenString;

        var roles = await identityService.GetRolesAsync(user.Id);
        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email, roles, user.DisplayName);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = newRefreshTokenString,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await tokenRepo.AddAsync(newRefreshToken, ct);
        await tokenRepo.SaveChangesAsync(ct);

        return AuthResult<TokenPairDto>.Ok(new TokenPairDto(accessToken, newRefreshTokenString, newRefreshToken.ExpiresAt));
    }
}
