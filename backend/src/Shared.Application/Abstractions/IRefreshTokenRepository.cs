using Shared.Domain.Identity;

namespace Shared.Application.Abstractions;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> FindByTokenAsync(string token, CancellationToken ct = default);
    Task AddAsync(RefreshToken token, CancellationToken ct = default);
    Task RevokeAllActiveForUserAsync(Guid userId, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
