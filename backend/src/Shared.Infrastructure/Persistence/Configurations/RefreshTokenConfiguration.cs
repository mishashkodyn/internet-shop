using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Identity;

namespace Shared.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(256);

        // Unique index — prevents duplicate tokens and enables O(log n) lookup by token value.
        builder.HasIndex(x => x.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshTokens_Token");

        // Non-unique index — fast per-user queries (e.g. revoke all tokens on logout).
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_RefreshTokens_UserId");
    }
}
