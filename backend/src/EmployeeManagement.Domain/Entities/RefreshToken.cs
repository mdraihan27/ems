using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

/// <summary>
/// Represents a refresh token for JWT authentication with single-use rotation.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// The refresh token string value.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// The user ID this token belongs to.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Expiration date/time of the token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Whether this token has been revoked.
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Whether this token has been used (single-use enforcement).
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// The token that replaced this one during rotation.
    /// </summary>
    public string? ReplacedByToken { get; set; }

    /// <summary>
    /// Checks if the token is still active (not expired, not revoked, not used).
    /// </summary>
    public bool IsActive => !IsRevoked && !IsUsed && ExpiresAt > DateTime.UtcNow;
}
