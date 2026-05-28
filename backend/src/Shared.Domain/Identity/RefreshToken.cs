namespace Shared.Domain.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }

    /// <summary>FK to AspNetUsers.</summary>
    public Guid UserId { get; set; }

    /// <summary>Opaque base64 token string, max 256 chars.</summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>UTC expiry time.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>UTC creation time.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>UTC revocation time; null means the token is still active.</summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>Populated when the token is rotated; null when simply revoked.</summary>
    public string? ReplacedByToken { get; set; }
}
