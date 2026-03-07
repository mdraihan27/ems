namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// DTO for displaying employee data in list and detail views.
/// </summary>
public class EmployeeDto
{
    /// <summary>Employee unique identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Employee full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>National ID number.</summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>Phone number in E.164 format.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Department name.</summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>Basic salary amount.</summary>
    public decimal BasicSalary { get; set; }

    /// <summary>Concurrency token.</summary>
    public uint RowVersion { get; set; }

    /// <summary>Spouse details if available.</summary>
    public SpouseDto? Spouse { get; set; }

    /// <summary>List of children.</summary>
    public List<ChildDto> Children { get; set; } = [];
}

/// <summary>
/// DTO for creating a new employee.
/// </summary>
public class CreateEmployeeDto
{
    /// <summary>Employee full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>National ID (10 or 17 digits).</summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>Bangladesh phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Department name.</summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>Basic salary.</summary>
    public decimal BasicSalary { get; set; }
}

/// <summary>
/// DTO for updating an existing employee.
/// </summary>
public class UpdateEmployeeDto
{
    /// <summary>Employee full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>National ID (10 or 17 digits).</summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>Bangladesh phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Department name.</summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>Basic salary.</summary>
    public decimal BasicSalary { get; set; }

    /// <summary>Concurrency token for optimistic concurrency.</summary>
    public uint RowVersion { get; set; }
}
