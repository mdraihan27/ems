using FluentAssertions;
using FluentValidation.TestHelper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Validators;

namespace EmployeeManagement.Tests.Validators;

public class SpouseAndChildValidatorTests
{
    private readonly UpsertSpouseDtoValidator _spouseValidator = new();
    private readonly CreateChildDtoValidator _createChildValidator = new();
    private readonly UpdateChildDtoValidator _updateChildValidator = new();

    // --- Spouse Validator Tests ---

    [Fact]
    public void Spouse_Should_Pass_When_Valid()
    {
        var dto = new UpsertSpouseDto { Name = "Fatema Begum", NID = "1234567890" };
        var result = _spouseValidator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Spouse_Should_Fail_When_Name_Empty()
    {
        var dto = new UpsertSpouseDto { Name = "", NID = "1234567890" };
        var result = _spouseValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Spouse_Should_Fail_When_NID_Invalid_Length()
    {
        var dto = new UpsertSpouseDto { Name = "Fatema", NID = "12345" };
        var result = _spouseValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NID);
    }

    [Fact]
    public void Spouse_Should_Fail_When_NID_Has_Letters()
    {
        var dto = new UpsertSpouseDto { Name = "Fatema", NID = "123456789A" };
        var result = _spouseValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.NID);
    }

    // --- CreateChild Validator Tests ---

    [Fact]
    public void CreateChild_Should_Pass_When_Valid()
    {
        var dto = new CreateChildDto
        {
            Name = "Rahman",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5))
        };
        var result = _createChildValidator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateChild_Should_Fail_When_Name_Empty()
    {
        var dto = new CreateChildDto { Name = "", DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)) };
        var result = _createChildValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateChild_Should_Fail_When_DOB_In_Future()
    {
        var dto = new CreateChildDto
        {
            Name = "Rahman",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };
        var result = _createChildValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    // --- UpdateChild Validator Tests ---

    [Fact]
    public void UpdateChild_Should_Pass_When_Valid()
    {
        var dto = new UpdateChildDto
        {
            Name = "Updated Name",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-3)),
            RowVersion = 1
        };
        var result = _updateChildValidator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateChild_Should_Fail_When_DOB_In_Future()
    {
        var dto = new UpdateChildDto
        {
            Name = "Child",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
            RowVersion = 1
        };
        var result = _updateChildValidator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }
}
