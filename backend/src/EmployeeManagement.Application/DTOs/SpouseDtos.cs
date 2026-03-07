namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// DTO for displaying spouse data.
/// </summary>
public class SpouseDto
{
    /// <summary>Spouse unique identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>Spouse full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Spouse National ID number.</summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>Concurrency token.</summary>
    public uint RowVersion { get; set; }
}

/// <summary>
/// DTO for creating or updating a spouse.
/// </summary>
public class UpsertSpouseDto
{
    /// <summary>Spouse full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Spouse National ID (10 or 17 digits).</summary>
    public string NID { get; set; } = string.Empty;

    /// <summary>Concurrency token (required for updates).</summary>
    public uint? RowVersion { get; set; }
}
