using System.Text.RegularExpressions;

namespace EmployeeManagement.Application.Helpers;

/// <summary>
/// Utility class for Bangladesh-specific phone number normalization.
/// </summary>
public static partial class PhoneNormalizer
{
    /// <summary>
    /// Regex pattern matching valid Bangladesh phone numbers.
    /// </summary>
    private static readonly Regex BdPhoneRegex = new(
        @"^(?:\+8801|01)\d{9}$", RegexOptions.Compiled);

    /// <summary>
    /// Validates whether a phone number matches Bangladesh format.
    /// </summary>
    public static bool IsValidBangladeshPhone(string phone)
    {
        return !string.IsNullOrWhiteSpace(phone) && BdPhoneRegex.IsMatch(phone);
    }

    /// <summary>
    /// Normalizes a Bangladesh phone number to E.164 format (+8801XXXXXXXXX).
    /// </summary>
    public static string NormalizeToE164(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return phone;

        phone = phone.Trim();
        if (phone.StartsWith("01"))
        {
            return "+88" + phone;
        }
        return phone;
    }
}
