using FluentValidation;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Helpers;

namespace EmployeeManagement.Application.Validators;

/// <summary>
/// Validator for CreateEmployeeDto with Bangladesh-specific rules.
/// </summary>
public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    /// <summary>
    /// Initializes validation rules for creating an employee.
    /// </summary>
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.NID)
            .NotEmpty().WithMessage("NID is required")
            .Must(nid => nid.Length == 10 || nid.Length == 17)
            .WithMessage("NID must be exactly 10 or 17 digits")
            .Matches(@"^\d+$").WithMessage("NID must contain only digits");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Must(PhoneNormalizer.IsValidBangladeshPhone)
            .WithMessage("Invalid Bangladesh phone format. Use +8801XXXXXXXXX or 01XXXXXXXXX");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(100).WithMessage("Department cannot exceed 100 characters");

        RuleFor(x => x.BasicSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Basic salary cannot be negative");
    }
}

/// <summary>
/// Validator for UpdateEmployeeDto with Bangladesh-specific rules.
/// </summary>
public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    /// <summary>
    /// Initializes validation rules for updating an employee.
    /// </summary>
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.NID)
            .NotEmpty().WithMessage("NID is required")
            .Must(nid => nid.Length == 10 || nid.Length == 17)
            .WithMessage("NID must be exactly 10 or 17 digits")
            .Matches(@"^\d+$").WithMessage("NID must contain only digits");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Must(PhoneNormalizer.IsValidBangladeshPhone)
            .WithMessage("Invalid Bangladesh phone format. Use +8801XXXXXXXXX or 01XXXXXXXXX");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(100).WithMessage("Department cannot exceed 100 characters");

        RuleFor(x => x.BasicSalary)
            .GreaterThanOrEqualTo(0).WithMessage("Basic salary cannot be negative");
    }
}

/// <summary>
/// Validator for UpsertSpouseDto.
/// </summary>
public class UpsertSpouseDtoValidator : AbstractValidator<UpsertSpouseDto>
{
    /// <summary>
    /// Initializes validation rules for creating/updating a spouse.
    /// </summary>
    public UpsertSpouseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Spouse name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.NID)
            .NotEmpty().WithMessage("Spouse NID is required")
            .Must(nid => nid.Length == 10 || nid.Length == 17)
            .WithMessage("NID must be exactly 10 or 17 digits")
            .Matches(@"^\d+$").WithMessage("NID must contain only digits");
    }
}

/// <summary>
/// Validator for CreateChildDto.
/// </summary>
public class CreateChildDtoValidator : AbstractValidator<CreateChildDto>
{
    /// <summary>
    /// Initializes validation rules for creating a child.
    /// </summary>
    public CreateChildDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Child name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.DateOfBirth)
            .Must(dob => dob <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth cannot be in the future");
    }
}

/// <summary>
/// Validator for UpdateChildDto.
/// </summary>
public class UpdateChildDtoValidator : AbstractValidator<UpdateChildDto>
{
    /// <summary>
    /// Initializes validation rules for updating a child.
    /// </summary>
    public UpdateChildDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Child name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.DateOfBirth)
            .Must(dob => dob <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth cannot be in the future");
    }
}

/// <summary>
/// Validator for LoginDto.
/// </summary>
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    /// <summary>
    /// Initializes validation rules for login.
    /// </summary>
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}

/// <summary>
/// Validator for RegisterDto.
/// </summary>
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    /// <summary>
    /// Initializes validation rules for registration.
    /// </summary>
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one digit")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters");

        RuleFor(x => x.Role)
            .Must(r => r == "Admin" || r == "Viewer")
            .WithMessage("Role must be either 'Admin' or 'Viewer'");
    }
}
