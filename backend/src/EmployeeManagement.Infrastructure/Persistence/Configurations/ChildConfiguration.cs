using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Child entity.
/// </summary>
public class ChildConfiguration : IEntityTypeConfiguration<Child>
{
    /// <summary>
    /// Configures Child table schema and constraints.
    /// </summary>
    public void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.ToTable("Children");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.RowVersion)
            .IsRowVersion();

        builder.HasIndex(c => c.EmployeeId)
            .HasDatabaseName("IX_Children_EmployeeId");
    }
}
