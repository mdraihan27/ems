using FluentAssertions;
using FluentValidation.TestHelper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Validators;

namespace EmployeeManagement.Tests.Validators;

public class CreateEmployeeDtoValidatorTests
{
    private readonly CreateEmployeeDtoValidator _validator = new();

    private static CreateEmployeeDto ValidDto() => new()
    {
        Name = "Md. Hasan Ali",
        NID = "1234567890",
        Phone = "+8801712345678",
        Department = "Engineering",
        BasicSalary = 50000m
    };

    [Fact]
    public void Should_Pass_When_All_Fields_Valid()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Fail_When_Name_Empty(string? name)
    {
        var dto = ValidDto();
        dto.Name = name!;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Name_Exceeds_200_Characters()
    {
        var dto = ValidDto();
        dto.Name = new string('A', 201);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("123456789")]     // 9 digits
    [InlineData("12345678901")]   // 11 digits
    public void Should_Fail_When_NID_Not_10_Or_17_Digits(string nid)
    {
        var dto = ValidDto();
        dto.NID = nid;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NID);
    }

    [Theory]
    [InlineData("1234567890")]         // 10 digits
    [InlineData("12345678901234567")]  // 17 digits
    public void Should_Pass_When_NID_Is_10_Or_17_Digits(string nid)
    {
        var dto = ValidDto();
        dto.NID = nid;
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.NID);
    }

    [Fact]
    public void Should_Fail_When_NID_Contains_Letters()
    {
        var dto = ValidDto();
        dto.NID = "12345ABCDE";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NID);
    }

    [Theory]
    [InlineData("+8801712345678")]
    [InlineData("01712345678")]
    public void Should_Pass_When_Phone_Valid_Bangladesh_Format(string phone)
    {
        var dto = ValidDto();
        dto.Phone = phone;
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Phone);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("+1234567890")]
    [InlineData("")]
    public void Should_Fail_When_Phone_Invalid(string phone)
    {
        var dto = ValidDto();
        dto.Phone = phone;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_Fail_When_Department_Empty()
    {
        var dto = ValidDto();
        dto.Department = "";
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Department);
    }

    [Fact]
    public void Should_Fail_When_Salary_Negative()
    {
        var dto = ValidDto();
        dto.BasicSalary = -1;
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.BasicSalary);
    }

    [Fact]
    public void Should_Pass_When_Salary_Zero()
    {
        var dto = ValidDto();
        dto.BasicSalary = 0;
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.BasicSalary);
    }
}
