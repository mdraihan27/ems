using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Infrastructure.Identity;

/// <summary>
/// Service implementing authentication operations including JWT token management.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    /// <summary>
    /// Initializes the auth service with required dependencies.
    /// </summary>
    public AuthService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IOptions<JwtSettings> jwtSettings,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _context = context;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _logger.LogWarning("Failed login attempt for email: {Email}", dto.Email);
            throw new AuthenticationFailedException("Invalid email or password.");
        }

        // NOTE: No email-confirmed check here because email verification is intentionally
        // skipped — this project was built for the Fionetix job interview assessment.

        _logger.LogInformation("Successful login for user: {Email}", dto.Email);
        return await GenerateAuthResponseAsync(user);
    }

    /// <inheritdoc />
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new DuplicateException($"A user with email '{dto.Email}' already exists.");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());
            throw new Shared.Exceptions.ValidationException(errors);
        }

        await _userManager.AddToRoleAsync(user, dto.Role);

        // NOTE: Email verification is intentionally skipped for this project.
        // This application was built as part of the Fionetix job interview assessment,
        // so newly registered accounts are immediately active without email confirmation.

        _logger.LogInformation("New user registered: {Email} with role {Role}", dto.Email, dto.Role);

        return await GenerateAuthResponseAsync(user);
    }

    /// <inheritdoc />
    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        if (principal == null)
            throw new AuthenticationFailedException("Invalid access token.");

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new AuthenticationFailedException("Invalid token claims.");

        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && rt.UserId == userId);

        if (storedToken == null || !storedToken.IsActive)
        {
            _logger.LogWarning("Invalid refresh token attempt for user: {UserId}", userId);
            throw new AuthenticationFailedException("Invalid or expired refresh token.");
        }

        // Mark the old token as used (single-use rotation)
        storedToken.IsUsed = true;
        storedToken.UpdatedAt = DateTime.UtcNow;

        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new AuthenticationFailedException("User not found.");

        var response = await GenerateAuthResponseAsync(user);

        // Link old token to new one
        storedToken.ReplacedByToken = response.RefreshToken;
        await _context.SaveChangesAsync();

        return response;
    }

    /// <inheritdoc />
    public async Task RevokeTokenAsync(RevokeTokenDto dto)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

        if (storedToken == null)
            throw new NotFoundException("Refresh token not found.");

        storedToken.IsRevoked = true;
        storedToken.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Refresh token revoked for user: {UserId}", storedToken.UserId);
    }

    /// <summary>
    /// Generates JWT access token and refresh token for a user.
    /// </summary>
    private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiry,
            Email = user.Email!,
            Roles = roles
        };
    }

    /// <summary>
    /// Creates and persists a new refresh token.
    /// </summary>
    private async Task<string> CreateRefreshTokenAsync(string userId)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var tokenString = Convert.ToBase64String(tokenBytes);

        var refreshToken = new RefreshToken
        {
            Token = tokenString,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return tokenString;
    }

    /// <summary>
    /// Extracts claims principal from an expired JWT token for refresh operations.
    /// </summary>
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }
}
