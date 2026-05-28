using Microsoft.EntityFrameworkCore;
using Shared.Application.Abstractions;
using Shared.Domain.Identity;

namespace Shared.Infrastructure.Persistence;

public class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public Task<RefreshToken?> FindByTokenAsync(string token, CancellationToken ct = default) =>
        db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token, ct);

    public async Task AddAsync(RefreshToken token, CancellationToken ct = default) =>
        await db.RefreshTokens.AddAsync(token, ct);

    public async Task RevokeAllActiveForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var tokens = await db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ToListAsync(ct);
        foreach (var t in tokens)
            t.RevokedAt = DateTime.UtcNow;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        db.SaveChangesAsync(ct);
}
