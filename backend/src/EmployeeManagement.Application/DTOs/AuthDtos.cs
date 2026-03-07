namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// DTO for login request.
/// </summary>
public class LoginDto
{
    /// <summary>User email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>User password.</summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO for user registration request.
/// </summary>
public class RegisterDto
{
    /// <summary>User email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>User password.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Full name of the user.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Role to assign (Admin or Viewer).</summary>
    public string Role { get; set; } = "Viewer";
}

/// <summary>
/// DTO for refresh token request.
/// </summary>
public class RefreshTokenDto
{
    /// <summary>Current access token.</summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Current refresh token.</summary>
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// DTO for revoke token request.
/// </summary>
public class RevokeTokenDto
{
    /// <summary>Refresh token to revoke.</summary>
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// DTO for authentication response containing tokens.
/// </summary>
public class AuthResponseDto
{
    /// <summary>JWT access token.</summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Refresh token for obtaining new access tokens.</summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>Access token expiration time.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>User email.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>User roles.</summary>
    public IList<string> Roles { get; set; } = [];
}
