using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MediatR;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Features.Family.Commands;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Tests.Handlers;

public class FamilyCommandHandlerTests
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly IMapper _mapper;

    public FamilyCommandHandlerTests()
    {
        _repoMock = new Mock<IEmployeeRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
    }

    // --- CreateSpouse ---

    [Fact]
    public async Task CreateSpouse_Should_Succeed_When_No_Existing_Spouse()
    {
        var handler = new CreateSpouseCommandHandler(_repoMock.Object, _mapper);
        var empId = Guid.NewGuid();
        var dto = new UpsertSpouseDto { Name = "Fatema Begum", NID = "1234567890" };

        _repoMock.Setup(r => r.ExistsAsync(empId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetSpouseByEmployeeIdAsync(empId, It.IsAny<CancellationToken>())).ReturnsAsync((Spouse?)null);
        _repoMock.Setup(r => r.AddSpouseAsync(It.IsAny<Spouse>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Spouse s, CancellationToken _) => s);

        var result = await handler.Handle(new CreateSpouseCommand(empId, dto), CancellationToken.None);

        result.Name.Should().Be("Fatema Begum");
        result.NID.Should().Be("1234567890");
    }

    [Fact]
    public async Task CreateSpouse_Should_Throw_When_Employee_Not_Found()
    {
        var handler = new CreateSpouseCommandHandler(_repoMock.Object, _mapper);
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => handler.Handle(
            new CreateSpouseCommand(Guid.NewGuid(), new UpsertSpouseDto { Name = "X", NID = "1234567890" }),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateSpouse_Should_Throw_DuplicateException_When_Spouse_Exists()
    {
        var handler = new CreateSpouseCommandHandler(_repoMock.Object, _mapper);
        var empId = Guid.NewGuid();

        _repoMock.Setup(r => r.ExistsAsync(empId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetSpouseByEmployeeIdAsync(empId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Spouse { Name = "Existing", NID = "1111111111", EmployeeId = empId });

        var act = () => handler.Handle(
            new CreateSpouseCommand(empId, new UpsertSpouseDto { Name = "New", NID = "2222222222" }),
            CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateException>();
    }

    // --- DeleteSpouse ---

    [Fact]
    public async Task DeleteSpouse_Should_Succeed_When_Found()
    {
        var handler = new DeleteSpouseCommandHandler(_repoMock.Object);
        var empId = Guid.NewGuid();
        var spouse = new Spouse { Name = "Spouse", NID = "1234567890", EmployeeId = empId };

        _repoMock.Setup(r => r.ExistsAsync(empId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.GetSpouseByEmployeeIdAsync(empId, It.IsAny<CancellationToken>())).ReturnsAsync(spouse);
        _repoMock.Setup(r => r.DeleteSpouseAsync(spouse, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await handler.Handle(new DeleteSpouseCommand(empId), CancellationToken.None);

        result.Should().Be(Unit.Value);
        _repoMock.Verify(r => r.DeleteSpouseAsync(spouse, It.IsAny<CancellationToken>()), Times.Once);
    }

    // --- CreateChild ---

    [Fact]
    public async Task CreateChild_Should_Succeed()
    {
        var handler = new CreateChildCommandHandler(_repoMock.Object, _mapper);
        var empId = Guid.NewGuid();
        var dto = new CreateChildDto { Name = "Rahim", DateOfBirth = new DateOnly(2018, 5, 15) };

        _repoMock.Setup(r => r.ExistsAsync(empId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _repoMock.Setup(r => r.AddChildAsync(It.IsAny<Child>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Child c, CancellationToken _) => c);

        var result = await handler.Handle(new CreateChildCommand(empId, dto), CancellationToken.None);

        result.Name.Should().Be("Rahim");
        result.DateOfBirth.Should().Be(new DateOnly(2018, 5, 15));
    }

    [Fact]
    public async Task CreateChild_Should_Throw_When_Employee_Not_Found()
    {
        var handler = new CreateChildCommandHandler(_repoMock.Object, _mapper);
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var act = () => handler.Handle(
            new CreateChildCommand(Guid.NewGuid(), new CreateChildDto { Name = "Child", DateOfBirth = new DateOnly(2020, 1, 1) }),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // --- DeleteChild ---

    [Fact]
    public async Task DeleteChild_Should_Throw_When_Child_Not_Found()
    {
        var handler = new DeleteChildCommandHandler(_repoMock.Object);
        var empId = Guid.NewGuid();
        var childId = Guid.NewGuid();

        _repoMock.Setup(r => r.GetChildByIdAsync(empId, childId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Child?)null);

        var act = () => handler.Handle(new DeleteChildCommand(empId, childId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
