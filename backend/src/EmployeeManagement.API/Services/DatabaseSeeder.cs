using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Identity;
using EmployeeManagement.Infrastructure.Persistence;

namespace EmployeeManagement.API.Services;

/// <summary>
/// Database seeder that runs automatically at startup to seed roles, users, and sample data.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with initial roles, optional admin user, and sample employee data.
    /// Only seeds if the database is empty (idempotent).
    /// </summary>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        await SeedRolesAsync(roleManager);
        await SeedAdminUserAsync(userManager, configuration);
        await SeedEmployeesAsync(context);
    }

    /// <summary>
    /// Seeds the Admin and Viewer roles.
    /// </summary>
    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["Admin", "Viewer"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    /// <summary>
    /// Seeds an admin user if SEED_ADMIN environment variable is "true".
    /// Reads credentials from ADMIN_EMAIL and ADMIN_SEED_PASSWORD env vars.
    /// </summary>
    private static async Task SeedAdminUserAsync(
        UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        var seedAdmin = Environment.GetEnvironmentVariable("SEED_ADMIN");
        if (!string.Equals(seedAdmin, "true", StringComparison.OrdinalIgnoreCase))
            return;

        var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@ems.com";
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? string.Empty;

        if (string.IsNullOrEmpty(adminPassword))
            return;

        if (await userManager.FindByEmailAsync(adminEmail) != null)
            return;

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "System Administrator",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, "Admin");
    }

    /// <summary>
    /// Seeds 10 employees with realistic Bangladeshi names, some with spouse and children data.
    /// Only runs if the Employees table is empty.
    /// </summary>
    private static async Task SeedEmployeesAsync(ApplicationDbContext context)
    {
        if (await context.Employees.AnyAsync())
            return;

        var employees = new List<Employee>
        {
            new()
            {
                Name = "Hasan Ahmed",
                NID = "1234567890",
                Phone = "+8801712345678",
                Department = "Engineering",
                BasicSalary = 75000.00m,
                Spouse = new Spouse { Name = "Fatima Begum", NID = "9876543210" },
                Children = new List<Child>
                {
                    new() { Name = "Arif Hasan", DateOfBirth = new DateOnly(2015, 3, 15) },
                    new() { Name = "Sadia Hasan", DateOfBirth = new DateOnly(2018, 7, 22) }
                }
            },
            new()
            {
                Name = "Moushumi Akter",
                NID = "2345678901",
                Phone = "+8801812345678",
                Department = "Human Resources",
                BasicSalary = 65000.00m,
                Spouse = new Spouse { Name = "Kamal Hossain", NID = "8765432109" },
                Children = new List<Child>
                {
                    new() { Name = "Nusrat Kamal", DateOfBirth = new DateOnly(2016, 11, 5) }
                }
            },
            new()
            {
                Name = "Tanvir Rahman",
                NID = "3456789012",
                Phone = "+8801912345678",
                Department = "Finance",
                BasicSalary = 80000.00m,
                Spouse = new Spouse { Name = "Nazia Rahman", NID = "7654321098" }
            },
            new()
            {
                Name = "Rafiq Islam",
                NID = "4567890123",
                Phone = "+8801612345678",
                Department = "Marketing",
                BasicSalary = 60000.00m,
                Children = new List<Child>
                {
                    new() { Name = "Imran Rafiq", DateOfBirth = new DateOnly(2012, 1, 30) },
                    new() { Name = "Tamanna Rafiq", DateOfBirth = new DateOnly(2014, 9, 18) },
                    new() { Name = "Sajid Rafiq", DateOfBirth = new DateOnly(2019, 4, 7) }
                }
            },
            new()
            {
                Name = "Ayesha Siddika",
                NID = "5678901234",
                Phone = "+8801512345678",
                Department = "Engineering",
                BasicSalary = 72000.00m
            },
            new()
            {
                Name = "Shakib Al Hasan",
                NID = "6789012345",
                Phone = "+8801312345678",
                Department = "Operations",
                BasicSalary = 90000.00m,
                Spouse = new Spouse { Name = "Umme Shakib", NID = "6543210987" },
                Children = new List<Child>
                {
                    new() { Name = "Alaina Shakib", DateOfBirth = new DateOnly(2017, 6, 10) }
                }
            },
            new()
            {
                Name = "Jannat Ara",
                NID = "7890123456",
                Phone = "+8801412345678",
                Department = "Sales",
                BasicSalary = 55000.00m
            },
            new()
            {
                Name = "Fahim Chowdhury",
                NID = "8901234567",
                Phone = "+8801112345678",
                Department = "IT Support",
                BasicSalary = 58000.00m,
                Spouse = new Spouse { Name = "Ruma Chowdhury", NID = "5432109876" },
                Children = new List<Child>
                {
                    new() { Name = "Zarif Fahim", DateOfBirth = new DateOnly(2020, 2, 14) }
                }
            },
            new()
            {
                Name = "Sumaiya Khatun",
                NID = "9012345678",
                Phone = "+8801713456789",
                Department = "Administration",
                BasicSalary = 62000.00m,
                Spouse = new Spouse { Name = "Abdul Karim", NID = "4321098765" }
            },
            new()
            {
                Name = "Karim Uddin",
                NID = "12345678901234567",
                Phone = "+8801814567890",
                Department = "Engineering",
                BasicSalary = 85000.00m,
                Children = new List<Child>
                {
                    new() { Name = "Rahima Karim", DateOfBirth = new DateOnly(2013, 8, 25) },
                    new() { Name = "Rahim Karim", DateOfBirth = new DateOnly(2016, 12, 1) }
                }
            }
        };

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }
}
