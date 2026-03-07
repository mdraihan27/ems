using MediatR;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Features.Family.Commands;

/// <summary>
/// Command to add a spouse to an employee.
/// </summary>
public record CreateSpouseCommand(Guid EmployeeId, UpsertSpouseDto Spouse) : IRequest<SpouseDto>;

/// <summary>
/// Command to update an employee's spouse.
/// </summary>
public record UpdateSpouseCommand(Guid EmployeeId, UpsertSpouseDto Spouse) : IRequest<SpouseDto>;

/// <summary>
/// Command to delete an employee's spouse.
/// </summary>
public record DeleteSpouseCommand(Guid EmployeeId) : IRequest<Unit>;

/// <summary>
/// Command to add a child to an employee.
/// </summary>
public record CreateChildCommand(Guid EmployeeId, CreateChildDto Child) : IRequest<ChildDto>;

/// <summary>
/// Command to update a child record.
/// </summary>
public record UpdateChildCommand(Guid EmployeeId, Guid ChildId, UpdateChildDto Child) : IRequest<ChildDto>;

/// <summary>
/// Command to delete a child record.
/// </summary>
public record DeleteChildCommand(Guid EmployeeId, Guid ChildId) : IRequest<Unit>;
