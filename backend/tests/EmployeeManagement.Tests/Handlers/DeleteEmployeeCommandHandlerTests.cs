using FluentAssertions;
using Moq;
using MediatR;
using EmployeeManagement.Application.Features.Employees.Commands;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Tests.Handlers;

public class DeleteEmployeeCommandHandlerTests
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly DeleteEmployeeCommandHandler _handler;

    public DeleteEmployeeCommandHandlerTests()
    {
        _repoMock = new Mock<IEmployeeRepository>();
        _handler = new DeleteEmployeeCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_When_Employee_Exists()
    {
        var emp = new Employee { Id = Guid.NewGuid(), Name = "Test", NID = "1234567890", Phone = "+8801712345678", Department = "HR" };
        _repoMock.Setup(r => r.GetByIdAsync(emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emp);
        _repoMock.Setup(r => r.DeleteAsync(emp, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(new DeleteEmployeeCommand(emp.Id), CancellationToken.None);

        result.Should().Be(Unit.Value);
        _repoMock.Verify(r => r.DeleteAsync(emp, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Missing()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        var act = () => _handler.Handle(new DeleteEmployeeCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
