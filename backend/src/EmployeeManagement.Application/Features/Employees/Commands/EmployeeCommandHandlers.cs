using AutoMapper;
using MediatR;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Helpers;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Features.Employees.Commands;

/// <summary>
/// Handler for CreateEmployeeCommand.
/// </summary>
public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler with required dependencies.
    /// </summary>
    public CreateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new employee after validating NID uniqueness and normalizing phone.
    /// </summary>
    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.NidExistsAsync(request.Employee.NID, cancellationToken: cancellationToken))
            throw new DuplicateException($"An employee with NID '{request.Employee.NID}' already exists.");

        var employee = _mapper.Map<Employee>(request.Employee);
        employee.Phone = PhoneNormalizer.NormalizeToE164(request.Employee.Phone);

        var created = await _repository.AddAsync(employee, cancellationToken);
        return _mapper.Map<EmployeeDto>(created);
    }
}

/// <summary>
/// Handler for UpdateEmployeeCommand.
/// </summary>
public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler with required dependencies.
    /// </summary>
    public UpdateEmployeeCommandHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Updates an existing employee after validating NID uniqueness and concurrency.
    /// </summary>
    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Employee", request.Id);

        if (await _repository.NidExistsAsync(request.Employee.NID, request.Id, cancellationToken))
            throw new DuplicateException($"An employee with NID '{request.Employee.NID}' already exists.");

        employee.RowVersion = request.Employee.RowVersion;
        _mapper.Map(request.Employee, employee);
        employee.Phone = PhoneNormalizer.NormalizeToE164(request.Employee.Phone);
        employee.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(employee, cancellationToken);

        var updated = await _repository.GetByIdNoTrackingAsync(request.Id, cancellationToken);
        return _mapper.Map<EmployeeDto>(updated!);
    }
}

/// <summary>
/// Handler for DeleteEmployeeCommand.
/// </summary>
public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IEmployeeRepository _repository;

    /// <summary>
    /// Initializes the handler with required dependencies.
    /// </summary>
    public DeleteEmployeeCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Deletes an employee and all related family data.
    /// </summary>
    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Employee", request.Id);

        await _repository.DeleteAsync(employee, cancellationToken);
        return Unit.Value;
    }
}
