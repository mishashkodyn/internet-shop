using MediatR;
using Shared.Application.Abstractions;
using Shared.Application.Auth;
using Shared.Application.Services;
using Shared.Domain.Identity;

namespace Shared.Application.Auth.Register;

public class RegisterCommandHandler(
    IIdentityService identityService,
    IRefreshTokenRepository tokenRepo,
    IJwtService jwtService) : IRequestHandler<RegisterCommand, AuthResult<TokenPairDto>>
{
    public async Task<AuthResult<TokenPairDto>> Handle(RegisterCommand cmd, CancellationToken ct)
    {
        var (success, userId, errors) = await identityService.CreateUserAsync(cmd.Email, cmd.Password, cmd.DisplayName);
        if (!success)
            return AuthResult<TokenPairDto>.Fail("REGISTRATION_FAILED", string.Join("; ", errors));

        await identityService.AddToRoleAsync(userId, "Customer");

        var roles = await identityService.GetRolesAsync(userId);
        var accessToken = jwtService.GenerateAccessToken(userId, cmd.Email, roles, cmd.DisplayName);
        var refreshTokenString = jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenString,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await tokenRepo.AddAsync(refreshToken, ct);
        await tokenRepo.SaveChangesAsync(ct);

        return AuthResult<TokenPairDto>.Ok(new TokenPairDto(accessToken, refreshTokenString, refreshToken.ExpiresAt));
    }
}
