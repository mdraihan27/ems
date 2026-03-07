using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Infrastructure.Identity;

/// <summary>
/// Application user extending ASP.NET Identity with additional properties.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
}
