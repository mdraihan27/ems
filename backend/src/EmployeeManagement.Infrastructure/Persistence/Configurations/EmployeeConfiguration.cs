using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the Employee entity.
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    /// <summary>
    /// Configures Employee table schema, indexes, and relationships.
    /// </summary>
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.NID)
            .IsRequired()
            .HasMaxLength(17);

        builder.HasIndex(e => e.NID)
            .IsUnique()
            .HasDatabaseName("IX_Employees_NID");

        builder.Property(e => e.Phone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(e => e.Department)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.Department)
            .HasDatabaseName("IX_Employees_Department");

        builder.Property(e => e.BasicSalary)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.RowVersion)
            .IsRowVersion();

        // One-to-one: Employee -> Spouse
        builder.HasOne(e => e.Spouse)
            .WithOne(s => s.Employee)
            .HasForeignKey<Spouse>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: Employee -> Children
        builder.HasMany(e => e.Children)
            .WithOne(c => c.Employee)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for search performance
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_Employees_Name");
    }
}
