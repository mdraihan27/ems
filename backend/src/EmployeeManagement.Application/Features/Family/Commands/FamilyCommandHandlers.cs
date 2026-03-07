using AutoMapper;
using MediatR;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Application.Features.Family.Commands;

/// <summary>
/// Handler for CreateSpouseCommand.
/// </summary>
public class CreateSpouseCommandHandler : IRequestHandler<CreateSpouseCommand, SpouseDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public CreateSpouseCommandHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a spouse for the specified employee.
    /// </summary>
    public async Task<SpouseDto> Handle(CreateSpouseCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.EmployeeId, cancellationToken))
            throw new NotFoundException("Employee", request.EmployeeId);

        var existing = await _repository.GetSpouseByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        if (existing != null)
            throw new DuplicateException("Employee already has a spouse. Use PUT to update.");

        var spouse = _mapper.Map<Spouse>(request.Spouse);
        spouse.EmployeeId = request.EmployeeId;

        var created = await _repository.AddSpouseAsync(spouse, cancellationToken);
        return _mapper.Map<SpouseDto>(created);
    }
}

/// <summary>
/// Handler for UpdateSpouseCommand.
/// </summary>
public class UpdateSpouseCommandHandler : IRequestHandler<UpdateSpouseCommand, SpouseDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public UpdateSpouseCommandHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Updates the spouse for the specified employee.
    /// </summary>
    public async Task<SpouseDto> Handle(UpdateSpouseCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.EmployeeId, cancellationToken))
            throw new NotFoundException("Employee", request.EmployeeId);

        var spouse = await _repository.GetSpouseByEmployeeIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Spouse not found for this employee.");

        if (request.Spouse.RowVersion.HasValue)
            spouse.RowVersion = request.Spouse.RowVersion.Value;

        _mapper.Map(request.Spouse, spouse);
        spouse.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateSpouseAsync(spouse, cancellationToken);

        var updated = await _repository.GetSpouseByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        return _mapper.Map<SpouseDto>(updated!);
    }
}

/// <summary>
/// Handler for DeleteSpouseCommand.
/// </summary>
public class DeleteSpouseCommandHandler : IRequestHandler<DeleteSpouseCommand, Unit>
{
    private readonly IEmployeeRepository _repository;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public DeleteSpouseCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Deletes the spouse of the specified employee.
    /// </summary>
    public async Task<Unit> Handle(DeleteSpouseCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.EmployeeId, cancellationToken))
            throw new NotFoundException("Employee", request.EmployeeId);

        var spouse = await _repository.GetSpouseByEmployeeIdAsync(request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Spouse not found for this employee.");

        await _repository.DeleteSpouseAsync(spouse, cancellationToken);
        return Unit.Value;
    }
}

/// <summary>
/// Handler for CreateChildCommand.
/// </summary>
public class CreateChildCommandHandler : IRequestHandler<CreateChildCommand, ChildDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public CreateChildCommandHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Adds a child to the specified employee.
    /// </summary>
    public async Task<ChildDto> Handle(CreateChildCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.EmployeeId, cancellationToken))
            throw new NotFoundException("Employee", request.EmployeeId);

        var child = _mapper.Map<Child>(request.Child);
        child.EmployeeId = request.EmployeeId;

        var created = await _repository.AddChildAsync(child, cancellationToken);
        return _mapper.Map<ChildDto>(created);
    }
}

/// <summary>
/// Handler for UpdateChildCommand.
/// </summary>
public class UpdateChildCommandHandler : IRequestHandler<UpdateChildCommand, ChildDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public UpdateChildCommandHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Updates an existing child record.
    /// </summary>
    public async Task<ChildDto> Handle(UpdateChildCommand request, CancellationToken cancellationToken)
    {
        var child = await _repository.GetChildByIdAsync(request.EmployeeId, request.ChildId, cancellationToken)
            ?? throw new NotFoundException("Child", request.ChildId);

        child.RowVersion = request.Child.RowVersion;
        _mapper.Map(request.Child, child);
        child.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateChildAsync(child, cancellationToken);

        var updated = await _repository.GetChildByIdAsync(request.EmployeeId, request.ChildId, cancellationToken);
        return _mapper.Map<ChildDto>(updated!);
    }
}

/// <summary>
/// Handler for DeleteChildCommand.
/// </summary>
public class DeleteChildCommandHandler : IRequestHandler<DeleteChildCommand, Unit>
{
    private readonly IEmployeeRepository _repository;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public DeleteChildCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Deletes a child from the specified employee.
    /// </summary>
    public async Task<Unit> Handle(DeleteChildCommand request, CancellationToken cancellationToken)
    {
        var child = await _repository.GetChildByIdAsync(request.EmployeeId, request.ChildId, cancellationToken)
            ?? throw new NotFoundException("Child", request.ChildId);

        await _repository.DeleteChildAsync(child, cancellationToken);
        return Unit.Value;
    }
}
