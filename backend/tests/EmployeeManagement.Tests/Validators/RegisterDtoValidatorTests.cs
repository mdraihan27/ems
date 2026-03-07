using FluentAssertions;
using FluentValidation.TestHelper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Validators;

namespace EmployeeManagement.Tests.Validators;

public class RegisterDtoValidatorTests
{
    private readonly RegisterDtoValidator _validator = new();

    private static RegisterDto ValidDto() => new()
    {
        Email = "admin@example.com",
        Password = "P@ssword1",
        FullName = "Admin User",
        Role = "Admin"
    };

    [Fact]
    public void Should_Pass_When_All_Fields_Valid()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Should_Fail_When_Email_Invalid(string email)
    {
        var dto = ValidDto();
        dto.Email = email;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("short1A!")]       // exactly 8, should pass
    [InlineData("S3cur3P@ss!")]    // valid
    public void Should_Pass_When_Password_Meets_Requirements(string password)
    {
        var dto = ValidDto();
        dto.Password = password;
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_Too_Short()
    {
        var dto = ValidDto();
        dto.Password = "P@ss1";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_No_Uppercase()
    {
        var dto = ValidDto();
        dto.Password = "p@ssword1";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_No_Lowercase()
    {
        var dto = ValidDto();
        dto.Password = "P@SSWORD1";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_No_Digit()
    {
        var dto = ValidDto();
        dto.Password = "P@ssword!";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_No_Special_Character()
    {
        var dto = ValidDto();
        dto.Password = "Password1";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Viewer")]
    public void Should_Pass_When_Role_Valid(string role)
    {
        var dto = ValidDto();
        dto.Role = role;
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }

    [Theory]
    [InlineData("SuperAdmin")]
    [InlineData("admin")]
    [InlineData("")]
    public void Should_Fail_When_Role_Invalid(string role)
    {
        var dto = ValidDto();
        dto.Role = role;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Should_Fail_When_FullName_Empty()
    {
        var dto = ValidDto();
        dto.FullName = "";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }
}
