using MediatR;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Shared.Models;

namespace EmployeeManagement.Application.Features.Employees.Queries;

/// <summary>
/// Query to retrieve an employee by ID.
/// </summary>
public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDto>;

/// <summary>
/// Query to retrieve all employees with pagination and sorting.
/// </summary>
public record GetAllEmployeesQuery(PaginationQuery Pagination) : IRequest<PaginatedList<EmployeeDto>>;

/// <summary>
/// Query to search employees by name, NID, or department.
/// </summary>
public record SearchEmployeeQuery(string? Query, PaginationQuery Pagination) : IRequest<PaginatedList<EmployeeDto>>;
