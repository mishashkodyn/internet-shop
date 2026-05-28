using MediatR;
using Shared.Application.Abstractions;

namespace Shared.Application.Auth.Logout;

public class LogoutCommandHandler(IRefreshTokenRepository tokenRepo) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand cmd, CancellationToken ct)
    {
        var token = await tokenRepo.FindByTokenAsync(cmd.RefreshToken, ct);
        if (token is null) return; // idempotent

        if (token.RevokedAt is null)
        {
            token.RevokedAt = DateTime.UtcNow;
            await tokenRepo.SaveChangesAsync(ct);
        }
    }
}
