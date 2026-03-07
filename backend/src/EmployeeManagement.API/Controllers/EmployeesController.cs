using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Features.Employees.Commands;
using EmployeeManagement.Application.Features.Employees.Queries;
using EmployeeManagement.Application.Features.Family.Commands;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Models;

namespace EmployeeManagement.API.Controllers;

/// <summary>
/// API controller for managing employees and their family members.
/// </summary>
[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEmployeeRepository _repository;
    private readonly IReportService _reportService;

    /// <summary>
    /// Initializes the controller with required services.
    /// </summary>
    public EmployeesController(IMediator mediator, IEmployeeRepository repository, IReportService reportService)
    {
        _mediator = mediator;
        _repository = repository;
        _reportService = reportService;
    }

    /// <summary>
    /// Gets all employees with pagination, sorting, and optional search.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] PaginationQuery pagination)
    {
        PaginatedList<EmployeeDto> result;

        if (!string.IsNullOrWhiteSpace(q))
            result = await _mediator.Send(new SearchEmployeeQuery(q, pagination));
        else
            result = await _mediator.Send(new GetAllEmployeesQuery(pagination));

        return Ok(ApiResponse<PaginatedList<EmployeeDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Gets a single employee by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id));
        return Ok(ApiResponse<EmployeeDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Searches employees by name, NID, or department.
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] PaginationQuery pagination)
    {
        var result = await _mediator.Send(new SearchEmployeeQuery(q, pagination));
        return Ok(ApiResponse<PaginatedList<EmployeeDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Creates a new employee. Requires Admin role.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var result = await _mediator.Send(new CreateEmployeeCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<EmployeeDto>.SuccessResponse(result, "Employee created successfully"));
    }

    /// <summary>
    /// Updates an existing employee. Requires Admin role.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto dto)
    {
        var result = await _mediator.Send(new UpdateEmployeeCommand(id, dto));
        return Ok(ApiResponse<EmployeeDto>.SuccessResponse(result, "Employee updated successfully"));
    }

    /// <summary>
    /// Deletes an employee and all related family data. Requires Admin role.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return Ok(ApiResponse<object>.SuccessResponse(null!, "Employee deleted successfully"));
    }

    // ===== SPOUSE ENDPOINTS =====

    /// <summary>
    /// Adds a spouse to an employee. Requires Admin role.
    /// </summary>
    [HttpPost("{id:guid}/spouse")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSpouse(Guid id, [FromBody] UpsertSpouseDto dto)
    {
        var result = await _mediator.Send(new CreateSpouseCommand(id, dto));
        return CreatedAtAction(nameof(GetById), new { id },
            ApiResponse<SpouseDto>.SuccessResponse(result, "Spouse added successfully"));
    }

    /// <summary>
    /// Updates an employee's spouse. Requires Admin role.
    /// </summary>
    [HttpPut("{id:guid}/spouse")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSpouse(Guid id, [FromBody] UpsertSpouseDto dto)
    {
        var result = await _mediator.Send(new UpdateSpouseCommand(id, dto));
        return Ok(ApiResponse<SpouseDto>.SuccessResponse(result, "Spouse updated successfully"));
    }

    /// <summary>
    /// Deletes an employee's spouse. Requires Admin role.
    /// </summary>
    [HttpDelete("{id:guid}/spouse")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSpouse(Guid id)
    {
        await _mediator.Send(new DeleteSpouseCommand(id));
        return Ok(ApiResponse<object>.SuccessResponse(null!, "Spouse deleted successfully"));
    }

    // ===== CHILDREN ENDPOINTS =====

    /// <summary>
    /// Adds a child to an employee. Requires Admin role.
    /// </summary>
    [HttpPost("{id:guid}/children")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateChild(Guid id, [FromBody] CreateChildDto dto)
    {
        var result = await _mediator.Send(new CreateChildCommand(id, dto));
        return CreatedAtAction(nameof(GetById), new { id },
            ApiResponse<ChildDto>.SuccessResponse(result, "Child added successfully"));
    }

    /// <summary>
    /// Updates a child record. Requires Admin role.
    /// </summary>
    [HttpPut("{id:guid}/children/{childId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateChild(Guid id, Guid childId, [FromBody] UpdateChildDto dto)
    {
        var result = await _mediator.Send(new UpdateChildCommand(id, childId, dto));
        return Ok(ApiResponse<ChildDto>.SuccessResponse(result, "Child updated successfully"));
    }

    /// <summary>
    /// Deletes a child record. Requires Admin role.
    /// </summary>
    [HttpDelete("{id:guid}/children/{childId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteChild(Guid id, Guid childId)
    {
        await _mediator.Send(new DeleteChildCommand(id, childId));
        return Ok(ApiResponse<object>.SuccessResponse(null!, "Child deleted successfully"));
    }

    // ===== REPORT ENDPOINTS =====

    /// <summary>
    /// Generates a PDF table report of employees matching an optional filter.
    /// </summary>
    [HttpGet("reports/list")]
    public async Task<IActionResult> DownloadListReport([FromQuery] string? q, [FromQuery] string? sortBy, [FromQuery] string sortDirection = "asc")
    {
        var isDescending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
        var employees = await _repository.GetAllForReportAsync(q, sortBy, isDescending, HttpContext.RequestAborted);
        var pdf = _reportService.GenerateEmployeeListReport(employees);
        return File(pdf, "application/pdf", "EmployeeReport.pdf");
    }

    /// <summary>
    /// Generates a PDF CV report for a specific employee including family details.
    /// </summary>
    [HttpGet("{id:guid}/reports/cv")]
    public async Task<IActionResult> DownloadCvReport(Guid id)
    {
        var employee = await _repository.GetByIdNoTrackingAsync(id, HttpContext.RequestAborted);
        if (employee == null)
            return NotFound(ApiResponse<object>.FailureResponse("Employee not found"));

        var pdf = _reportService.GenerateEmployeeCvReport(employee);
        return File(pdf, "application/pdf", $"CV_{employee.Name}.pdf");
    }
}
