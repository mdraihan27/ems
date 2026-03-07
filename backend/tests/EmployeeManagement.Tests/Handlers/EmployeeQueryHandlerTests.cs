using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Features.Employees.Queries;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Mappings;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Models;

namespace EmployeeManagement.Tests.Handlers;

public class EmployeeQueryHandlerTests
{
    private readonly Mock<IEmployeeRepository> _repoMock;
    private readonly IMapper _mapper;

    public EmployeeQueryHandlerTests()
    {
        _repoMock = new Mock<IEmployeeRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
    }

    // --- GetEmployeeById ---

    [Fact]
    public async Task GetById_Should_Return_Employee_When_Found()
    {
        var handler = new GetEmployeeByIdQueryHandler(_repoMock.Object, _mapper);
        var emp = new Employee { Id = Guid.NewGuid(), Name = "Hasan", NID = "1234567890", Phone = "+8801712345678", Department = "IT", BasicSalary = 50000 };

        _repoMock.Setup(r => r.GetByIdNoTrackingAsync(emp.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emp);

        var result = await handler.Handle(new GetEmployeeByIdQuery(emp.Id), CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(emp.Id);
        result.Name.Should().Be("Hasan");
    }

    [Fact]
    public async Task GetById_Should_Throw_NotFoundException_When_Missing()
    {
        var handler = new GetEmployeeByIdQueryHandler(_repoMock.Object, _mapper);
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdNoTrackingAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        var act = () => handler.Handle(new GetEmployeeByIdQuery(id), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // --- GetAllEmployees ---

    [Fact]
    public async Task GetAll_Should_Return_Paginated_List()
    {
        var handler = new GetAllEmployeesQueryHandler(_repoMock.Object, _mapper);
        var employees = new List<Employee>
        {
            new() { Name = "Hasan", NID = "1234567890", Phone = "+8801712345678", Department = "IT", BasicSalary = 50000 },
            new() { Name = "Tanvir", NID = "9876543210", Phone = "+8801812345678", Department = "HR", BasicSalary = 45000 }
        };

        _repoMock.Setup(r => r.GetAllAsync(1, 20, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((employees, 2));

        var pagination = new PaginationQuery { Page = 1, PageSize = 20 };
        var result = await handler.Handle(new GetAllEmployeesQuery(pagination), CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.Pagination.TotalCount.Should().Be(2);
        result.Pagination.Page.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_Should_Return_Empty_When_No_Employees()
    {
        var handler = new GetAllEmployeesQueryHandler(_repoMock.Object, _mapper);
        _repoMock.Setup(r => r.GetAllAsync(1, 20, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Employee>(), 0));

        var result = await handler.Handle(new GetAllEmployeesQuery(new PaginationQuery()), CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.Pagination.TotalCount.Should().Be(0);
    }

    // --- SearchEmployees ---

    [Fact]
    public async Task Search_Should_Filter_By_Query()
    {
        var handler = new SearchEmployeeQueryHandler(_repoMock.Object, _mapper);
        var employees = new List<Employee>
        {
            new() { Name = "Hasan", NID = "1234567890", Phone = "+8801712345678", Department = "IT", BasicSalary = 50000 }
        };

        _repoMock.Setup(r => r.SearchAsync("Hasan", 1, 20, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((employees, 1));

        var pagination = new PaginationQuery { Page = 1, PageSize = 20 };
        var result = await handler.Handle(new SearchEmployeeQuery("Hasan", pagination), CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.Items[0].Name.Should().Be("Hasan");
    }

    [Fact]
    public async Task Search_Should_Use_Empty_String_When_Query_Null()
    {
        var handler = new SearchEmployeeQueryHandler(_repoMock.Object, _mapper);
        _repoMock.Setup(r => r.SearchAsync(string.Empty, 1, 20, null, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Employee>(), 0));

        var result = await handler.Handle(new SearchEmployeeQuery(null, new PaginationQuery()), CancellationToken.None);

        result.Items.Should().BeEmpty();
        _repoMock.Verify(r => r.SearchAsync(string.Empty, 1, 20, null, false, It.IsAny<CancellationToken>()), Times.Once);
    }
}
