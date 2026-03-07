namespace EmployeeManagement.Infrastructure.Identity;

/// <summary>
/// JWT configuration settings loaded from appsettings/environment.
/// </summary>
public class JwtSettings
{
    /// <summary>Configuration section name.</summary>
    public const string SectionName = "JwtSettings";

    /// <summary>Secret key for signing tokens.</summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>Token issuer.</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Token audience.</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>Access token expiry in minutes (default: 15).</summary>
    public int AccessTokenExpiryMinutes { get; set; } = 15;

    /// <summary>Refresh token expiry in days (default: 30).</summary>
    public int RefreshTokenExpiryDays { get; set; } = 30;
}
