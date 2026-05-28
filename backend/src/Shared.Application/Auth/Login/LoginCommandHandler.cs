using MediatR;
using Shared.Application.Abstractions;
using Shared.Application.Auth;
using Shared.Application.Services;
using Shared.Domain.Identity;

namespace Shared.Application.Auth.Login;

public class LoginCommandHandler(
    IIdentityService identityService,
    IRefreshTokenRepository tokenRepo,
    IJwtService jwtService) : IRequestHandler<LoginCommand, AuthResult<TokenPairDto>>
{
    private const string InvalidCredentials = "The email or password is incorrect.";

    public async Task<AuthResult<TokenPairDto>> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await identityService.FindByEmailAsync(cmd.Email);
        if (user is null)
            return AuthResult<TokenPairDto>.Fail("INVALID_CREDENTIALS", InvalidCredentials);

        if (await identityService.IsLockedOutAsync(user.Id))
            return AuthResult<TokenPairDto>.Fail("ACCOUNT_LOCKED", "Account is temporarily locked.");

        var passwordValid = await identityService.CheckPasswordAsync(user.Id, cmd.Password);
        if (!passwordValid)
        {
            await identityService.RecordFailedLoginAsync(user.Id);
            return AuthResult<TokenPairDto>.Fail("INVALID_CREDENTIALS", InvalidCredentials);
        }

        await identityService.ResetFailedLoginCountAsync(user.Id);

        var roles = await identityService.GetRolesAsync(user.Id);
        var accessToken = jwtService.GenerateAccessToken(user.Id, user.Email, roles, user.DisplayName);
        var refreshTokenString = jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenString,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await tokenRepo.AddAsync(refreshToken, ct);
        await tokenRepo.SaveChangesAsync(ct);

        return AuthResult<TokenPairDto>.Ok(new TokenPairDto(accessToken, refreshTokenString, refreshToken.ExpiresAt));
    }
}
