using AutoMapper;
using MediatR;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Models;

namespace EmployeeManagement.Application.Features.Employees.Queries;

/// <summary>
/// Handler for GetEmployeeByIdQuery.
/// </summary>
public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler with required dependencies.
    /// </summary>
    public GetEmployeeByIdQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a single employee by ID.
    /// </summary>
    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdNoTrackingAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Employee", request.Id);

        return _mapper.Map<EmployeeDto>(employee);
    }
}

/// <summary>
/// Handler for GetAllEmployeesQuery.
/// </summary>
public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PaginatedList<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler with required dependencies.
    /// </summary>
    public GetAllEmployeesQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a paginated list of all employees.
    /// </summary>
    public async Task<PaginatedList<EmployeeDto>> Handle(
        GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetAllAsync(
            request.Pagination.Page,
            request.Pagination.PageSize,
            request.Pagination.SortBy,
            request.Pagination.IsDescending,
            cancellationToken);

        var dtos = _mapper.Map<List<EmployeeDto>>(items);
        return PaginatedList<EmployeeDto>.Create(
            dtos, totalCount, request.Pagination.Page, request.Pagination.PageSize);
    }
}

/// <summary>
/// Handler for SearchEmployeeQuery.
/// </summary>
public class SearchEmployeeQueryHandler : IRequestHandler<SearchEmployeeQuery, PaginatedList<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler with required dependencies.
    /// </summary>
    public SearchEmployeeQueryHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Searches employees by query string with pagination.
    /// </summary>
    public async Task<PaginatedList<EmployeeDto>> Handle(
        SearchEmployeeQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.SearchAsync(
            request.Query ?? string.Empty,
            request.Pagination.Page,
            request.Pagination.PageSize,
            request.Pagination.SortBy,
            request.Pagination.IsDescending,
            cancellationToken);

        var dtos = _mapper.Map<List<EmployeeDto>>(items);
        return PaginatedList<EmployeeDto>.Create(
            dtos, totalCount, request.Pagination.Page, request.Pagination.PageSize);
    }
}
