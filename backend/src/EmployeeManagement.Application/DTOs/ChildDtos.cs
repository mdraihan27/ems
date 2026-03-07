namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// DTO for displaying child data.
/// </summary>
public class ChildDto
{
    /// <summary>Child unique identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Child full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Date of birth.</summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>Concurrency token.</summary>
    public uint RowVersion { get; set; }
}

/// <summary>
/// DTO for creating a new child.
/// </summary>
public class CreateChildDto
{
    /// <summary>Child full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Date of birth.</summary>
    public DateOnly DateOfBirth { get; set; }
}

/// <summary>
/// DTO for updating an existing child.
/// </summary>
public class UpdateChildDto
{
    /// <summary>Child full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Date of birth.</summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>Concurrency token for optimistic concurrency.</summary>
    public uint RowVersion { get; set; }
}
