using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Common;

/// <summary>
/// Base entity class providing common properties for all domain entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Concurrency token for optimistic concurrency control.
    /// </summary>
    [Timestamp]
    public uint RowVersion { get; set; }
}
