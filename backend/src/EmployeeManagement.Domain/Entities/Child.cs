using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

/// <summary>
/// Represents a child of an employee (one-to-many relationship).
/// </summary>
public class Child : BaseEntity
{
    /// <summary>
    /// Full name of the child.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth of the child (date only, no time component).
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Foreign key reference to the parent employee.
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Navigation property to the parent employee.
    /// </summary>
    public Employee Employee { get; set; } = null!;
}
