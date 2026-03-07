using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

/// <summary>
/// Represents an employee in the system with personal and professional details.
/// </summary>
public class Employee : BaseEntity
{
    /// <summary>
    /// Full name of the employee.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// National ID (NID) - must be exactly 10 or 17 digits. Unique across the system.
    /// </summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>
    /// Phone number stored in E.164 format (+8801XXXXXXXXX).
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Department the employee belongs to.
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Basic salary of the employee.
    /// </summary>
    public decimal BasicSalary { get; set; }

    /// <summary>
    /// Navigation property for the employee's spouse (optional, one-to-one).
    /// </summary>
    public Spouse? Spouse { get; set; }

    /// <summary>
    /// Navigation property for the employee's children (one-to-many).
    /// </summary>
    public ICollection<Child> Children { get; set; } = new List<Child>();
}
