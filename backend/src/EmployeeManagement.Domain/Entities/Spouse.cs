using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

/// <summary>
/// Represents the spouse of an employee (one-to-one relationship).
/// </summary>
public class Spouse : BaseEntity
{
    /// <summary>
    /// Full name of the spouse.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// National ID (NID) of the spouse - must be exactly 10 or 17 digits.
    /// </summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key reference to the associated employee.
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Navigation property to the associated employee.
    /// </summary>
    public Employee Employee { get; set; } = null!;
}
