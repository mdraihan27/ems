using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Features.Employees.Commands;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Tests.Handlers;

public class UpdateEmployeeCommandHandlerTests
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly UpdateEmployeeCommandHandler _handler;

    public UpdateEmployeeCommandHandlerTests()
    {
        _repoMock = new Mock<IEmployeeRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
        _handler = new UpdateEmployeeCommandHandler(_repoMock.Object, _mapper);
    }

    private static Employee ExistingEmployee() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Old Name",
        NID = "1234567890",
        Phone = "+8801712345678",
        Department = "HR",
        BasicSalary = 40000m,
        RowVersion = 1
    };

    [Fact]
    public async Task Handle_Should_Update_When_Found_And_NID_Unique()
    {
        var emp = ExistingEmployee();
        var dto = new UpdateEmployeeDto
        {
            Name = "New Name",
            NID = "1234567890",
            Phone = "01712345678",
            Department = "Engineering",
            BasicSalary = 60000m,
            RowVersion = 1
        };

        _repoMock.Setup(r => r.GetByIdAsync(emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emp);
        _repoMock.Setup(r => r.NidExistsAsync(dto.NID, emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.GetByIdNoTrackingAsync(emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emp);

        var result = await _handler.Handle(new UpdateEmployeeCommand(emp.Id, dto), CancellationToken.None);

        result.Should().NotBeNull();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Employee_Missing()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        var dto = new UpdateEmployeeDto
        {
            Name = "N", NID = "1234567890", Phone = "+8801712345678",
            Department = "D", BasicSalary = 0, RowVersion = 1
        };

        var act = () => _handler.Handle(new UpdateEmployeeCommand(id, dto), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_DuplicateException_When_NID_Used_By_Another()
    {
        var emp = ExistingEmployee();
        var dto = new UpdateEmployeeDto
        {
            Name = "New", NID = "9999999999", Phone = "+8801712345678",
            Department = "D", BasicSalary = 0, RowVersion = 1
        };

        _repoMock.Setup(r => r.GetByIdAsync(emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emp);
        _repoMock.Setup(r => r.NidExistsAsync(dto.NID, emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var act = () => _handler.Handle(new UpdateEmployeeCommand(emp.Id, dto), CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateException>();
    }
}
