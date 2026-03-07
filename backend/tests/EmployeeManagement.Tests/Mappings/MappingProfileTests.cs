using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Tests.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_Should_Not_Throw_On_CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly), NullLoggerFactory.Instance);
        var mapper = config.CreateMapper();
        mapper.Should().NotBeNull();
    }

    [Fact]
    public void Should_Map_Employee_To_EmployeeDto()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Hasan",
            NID = "1234567890",
            Phone = "+8801712345678",
            Department = "IT",
            BasicSalary = 50000,
            RowVersion = 1,
            Spouse = new Spouse { Name = "Fatema", NID = "0987654321" },
            Children = [new Child { Name = "Rahim", DateOfBirth = new DateOnly(2020, 1, 1) }]
        };

        var dto = _mapper.Map<EmployeeDto>(employee);

        dto.Id.Should().Be(employee.Id);
        dto.Name.Should().Be("Hasan");
        dto.Spouse.Should().NotBeNull();
        dto.Spouse!.Name.Should().Be("Fatema");
        dto.Children.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Map_CreateEmployeeDto_To_Employee()
    {
        var dto = new CreateEmployeeDto
        {
            Name = "Tanvir",
            NID = "9876543210",
            Phone = "01812345678",
            Department = "HR",
            BasicSalary = 45000
        };

        var employee = _mapper.Map<Employee>(dto);

        employee.Name.Should().Be("Tanvir");
        employee.NID.Should().Be("9876543210");
        employee.Department.Should().Be("HR");
    }

    [Fact]
    public void Should_Map_UpdateEmployeeDto_To_Employee_Without_Overwriting_Id()
    {
        var existingEmployee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            NID = "1111111111",
            Phone = "+8801711111111",
            Department = "Old Dept",
            BasicSalary = 30000,
            CreatedAt = DateTime.UtcNow.AddDays(-30)
        };

        var dto = new UpdateEmployeeDto
        {
            Name = "New",
            NID = "2222222222",
            Phone = "+8801722222222",
            Department = "New Dept",
            BasicSalary = 60000
        };

        var originalId = existingEmployee.Id;
        var originalCreatedAt = existingEmployee.CreatedAt;
        _mapper.Map(dto, existingEmployee);

        existingEmployee.Id.Should().Be(originalId);
        existingEmployee.CreatedAt.Should().Be(originalCreatedAt);
        existingEmployee.Name.Should().Be("New");
        existingEmployee.Department.Should().Be("New Dept");
    }

    [Fact]
    public void Should_Map_UpsertSpouseDto_To_Spouse()
    {
        var dto = new UpsertSpouseDto { Name = "Spouse", NID = "1234567890" };
        var spouse = _mapper.Map<Spouse>(dto);

        spouse.Name.Should().Be("Spouse");
        spouse.NID.Should().Be("1234567890");
        spouse.EmployeeId.Should().Be(Guid.Empty); // Not mapped
    }

    [Fact]
    public void Should_Map_Child_To_ChildDto()
    {
        var child = new Child
        {
            Id = Guid.NewGuid(),
            Name = "Rahim",
            DateOfBirth = new DateOnly(2018, 5, 15),
            RowVersion = 2
        };

        var dto = _mapper.Map<ChildDto>(child);

        dto.Id.Should().Be(child.Id);
        dto.Name.Should().Be("Rahim");
        dto.DateOfBirth.Should().Be(new DateOnly(2018, 5, 15));
        dto.RowVersion.Should().Be(2u);
    }
}
