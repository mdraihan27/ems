using MediatR;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Features.Employees.Commands;

/// <summary>
/// Command to create a new employee.
/// </summary>
public record CreateEmployeeCommand(CreateEmployeeDto Employee) : IRequest<EmployeeDto>;

/// <summary>
/// Command to update an existing employee.
/// </summary>
public record UpdateEmployeeCommand(Guid Id, UpdateEmployeeDto Employee) : IRequest<EmployeeDto>;

/// <summary>
/// Command to delete an employee.
/// </summary>
public record DeleteEmployeeCommand(Guid Id) : IRequest<Unit>;
