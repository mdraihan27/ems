using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

/// <summary>
/// Service interface for authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user and returns access and refresh tokens.
    /// </summary>
    Task<AuthResponseDto> LoginAsync(LoginDto dto);

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

    /// <summary>
    /// Refreshes an expired access token using a valid refresh token.
    /// </summary>
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);

    /// <summary>
    /// Revokes a refresh token to prevent further use.
    /// </summary>
    Task RevokeTokenAsync(RevokeTokenDto dto);
}
