using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Identity;

namespace EmployeeManagement.Infrastructure.Persistence;

/// <summary>
/// Application database context integrating EF Core with ASP.NET Identity.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes the database context.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>Employees table.</summary>
    public DbSet<Employee> Employees => Set<Employee>();

    /// <summary>Spouses table.</summary>
    public DbSet<Spouse> Spouses => Set<Spouse>();

    /// <summary>Children table.</summary>
    public DbSet<Child> Children => Set<Child>();

    /// <summary>Refresh tokens table.</summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <summary>
    /// Configures entity relationships and database constraints.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
