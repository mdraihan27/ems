using FluentAssertions;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Tests.Domain;

public class EntityTests
{
    [Fact]
    public void Employee_Should_Initialize_With_Defaults()
    {
        var emp = new Employee();

        emp.Id.Should().NotBeEmpty();
        emp.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        emp.UpdatedAt.Should().BeNull();
        emp.Children.Should().NotBeNull().And.BeEmpty();
        emp.Spouse.Should().BeNull();
    }

    [Fact]
    public void Employee_Should_Hold_Family_Relationships()
    {
        var emp = new Employee
        {
            Name = "Test",
            NID = "1234567890",
            Phone = "+8801712345678",
            Department = "IT",
            BasicSalary = 50000
        };

        var spouse = new Spouse { Name = "Spouse", NID = "0987654321", EmployeeId = emp.Id };
        emp.Spouse = spouse;

        var child = new Child { Name = "Child", DateOfBirth = new DateOnly(2020, 1, 1), EmployeeId = emp.Id };
        emp.Children.Add(child);

        emp.Spouse.Should().NotBeNull();
        emp.Spouse!.Name.Should().Be("Spouse");
        emp.Children.Should().HaveCount(1);
    }

    [Fact]
    public void RefreshToken_IsActive_Should_Be_True_When_Valid()
    {
        var token = new RefreshToken
        {
            Token = "test-token",
            UserId = "user-1",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            IsUsed = false
        };

        token.IsActive.Should().BeTrue();
    }

    [Fact]
    public void RefreshToken_IsActive_Should_Be_False_When_Expired()
    {
        var token = new RefreshToken
        {
            Token = "test-token",
            UserId = "user-1",
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1),
            IsRevoked = false,
            IsUsed = false
        };

        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public void RefreshToken_IsActive_Should_Be_False_When_Revoked()
    {
        var token = new RefreshToken
        {
            Token = "test-token",
            UserId = "user-1",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = true,
            IsUsed = false
        };

        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public void RefreshToken_IsActive_Should_Be_False_When_Used()
    {
        var token = new RefreshToken
        {
            Token = "test-token",
            UserId = "user-1",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            IsUsed = true
        };

        token.IsActive.Should().BeFalse();
    }
}
