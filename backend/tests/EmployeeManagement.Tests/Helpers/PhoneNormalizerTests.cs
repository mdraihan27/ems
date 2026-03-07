using FluentAssertions;
using EmployeeManagement.Application.Helpers;

namespace EmployeeManagement.Tests.Helpers;

public class PhoneNormalizerTests
{
    [Theory]
    [InlineData("01712345678", true)]
    [InlineData("01812345678", true)]
    [InlineData("01912345678", true)]
    [InlineData("+8801712345678", true)]
    [InlineData("+8801812345678", true)]
    [InlineData("1234567890", false)]
    [InlineData("+1234567890", false)]
    [InlineData("", false)]
    [InlineData("017123456", false)]     // too short
    [InlineData("017123456789", false)]  // too long for 01X format
    public void IsValidBangladeshPhone_Should_Validate_Correctly(string phone, bool expected)
    {
        PhoneNormalizer.IsValidBangladeshPhone(phone).Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void IsValidBangladeshPhone_Should_Return_False_For_NullOrWhitespace(string? phone)
    {
        PhoneNormalizer.IsValidBangladeshPhone(phone!).Should().BeFalse();
    }

    [Theory]
    [InlineData("01712345678", "+8801712345678")]
    [InlineData("01812345678", "+8801812345678")]
    public void NormalizeToE164_Should_Prepend_88_For_01_Numbers(string input, string expected)
    {
        PhoneNormalizer.NormalizeToE164(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("+8801712345678", "+8801712345678")]
    [InlineData("+8801812345678", "+8801812345678")]
    public void NormalizeToE164_Should_Keep_Already_E164_Numbers(string input, string expected)
    {
        PhoneNormalizer.NormalizeToE164(input).Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void NormalizeToE164_Should_Return_Input_For_NullOrWhitespace(string? input)
    {
        PhoneNormalizer.NormalizeToE164(input!).Should().Be(input);
    }
}
