using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the RefreshToken entity.
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    /// <summary>
    /// Configures RefreshToken table schema and indexes.
    /// </summary>
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(r => r.Token)
            .HasDatabaseName("IX_RefreshTokens_Token");

        builder.Property(r => r.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasIndex(r => r.UserId)
            .HasDatabaseName("IX_RefreshTokens_UserId");
    }
}
