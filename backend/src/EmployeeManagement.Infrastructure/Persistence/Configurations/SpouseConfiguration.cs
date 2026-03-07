using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Spouse entity.
/// </summary>
public class SpouseConfiguration : IEntityTypeConfiguration<Spouse>
{
    /// <summary>
    /// Configures Spouse table schema and constraints.
    /// </summary>
    public void Configure(EntityTypeBuilder<Spouse> builder)
    {
        builder.ToTable("Spouses");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.NID)
            .IsRequired()
            .HasMaxLength(17);

        builder.Property(s => s.RowVersion)
            .IsRowVersion();

        builder.HasIndex(s => s.EmployeeId)
            .IsUnique()
            .HasDatabaseName("IX_Spouses_EmployeeId");
    }
}
