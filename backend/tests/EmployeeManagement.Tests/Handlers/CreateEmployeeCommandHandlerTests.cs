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

public class CreateEmployeeCommandHandlerTests
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly CreateEmployeeCommandHandler _handler;

    public CreateEmployeeCommandHandlerTests()
    {
        _repoMock = new Mock<IEmployeeRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
        _handler = new CreateEmployeeCommandHandler(_repoMock.Object, _mapper);
    }

    private static CreateEmployeeDto ValidDto() => new()
    {
        Name = "Md. Hasan Ali",
        NID = "1234567890",
        Phone = "01712345678",
        Department = "Engineering",
        BasicSalary = 50000m
    };

    [Fact]
    public async Task Handle_Should_Create_Employee_When_NID_Unique()
    {
        var dto = ValidDto();
        _repoMock.Setup(r => r.NidExistsAsync(dto.NID, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee e, CancellationToken _) => e);

        var result = await _handler.Handle(new CreateEmployeeCommand(dto), CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
        result.Phone.Should().Be("+8801712345678"); // normalized to E.164
        result.Department.Should().Be(dto.Department);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_DuplicateException_When_NID_Exists()
    {
        var dto = ValidDto();
        _repoMock.Setup(r => r.NidExistsAsync(dto.NID, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var act = () => _handler.Handle(new CreateEmployeeCommand(dto), CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateException>()
            .WithMessage($"*NID*{dto.NID}*");
    }

    [Fact]
    public async Task Handle_Should_Normalize_Phone_Starting_With_01()
    {
        var dto = ValidDto();
        dto.Phone = "01912345678";
        _repoMock.Setup(r => r.NidExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee e, CancellationToken _) => e);

        var result = await _handler.Handle(new CreateEmployeeCommand(dto), CancellationToken.None);

        result.Phone.Should().Be("+8801912345678");
    }

    [Fact]
    public async Task Handle_Should_Keep_Phone_Already_In_E164()
    {
        var dto = ValidDto();
        dto.Phone = "+8801812345678";
        _repoMock.Setup(r => r.NidExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee e, CancellationToken _) => e);

        var result = await _handler.Handle(new CreateEmployeeCommand(dto), CancellationToken.None);

        result.Phone.Should().Be("+8801812345678");
    }
}
