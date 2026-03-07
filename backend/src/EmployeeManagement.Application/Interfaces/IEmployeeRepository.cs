using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces;

/// <summary>
/// Repository interface for Employee data access operations.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Gets an employee by ID including related entities.
    /// </summary>
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an employee by ID without tracking (for read operations).
    /// </summary>
    Task<Employee?> GetByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all employees with pagination and optional sorting.
    /// </summary>
    Task<(List<Employee> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, string? sortBy, bool isDescending,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches employees by name, NID, or department.
    /// </summary>
    Task<(List<Employee> Items, int TotalCount)> SearchAsync(
        string query, int page, int pageSize, string? sortBy, bool isDescending,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an NID already exists in the system, optionally excluding a specific employee.
    /// </summary>
    Task<bool> NidExistsAsync(string nid, Guid? excludeEmployeeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new employee to the database.
    /// </summary>
    Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an employee from the database.
    /// </summary>
    Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the spouse of an employee.
    /// </summary>
    Task<Spouse?> GetSpouseByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a spouse to an employee.
    /// </summary>
    Task<Spouse> AddSpouseAsync(Spouse spouse, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a spouse record.
    /// </summary>
    Task UpdateSpouseAsync(Spouse spouse, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a spouse record.
    /// </summary>
    Task DeleteSpouseAsync(Spouse spouse, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a child by ID.
    /// </summary>
    Task<Child?> GetChildByIdAsync(Guid employeeId, Guid childId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a child to an employee.
    /// </summary>
    Task<Child> AddChildAsync(Child child, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a child record.
    /// </summary>
    Task UpdateChildAsync(Child child, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a child record.
    /// </summary>
    Task DeleteChildAsync(Child child, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all employees for PDF report generation.
    /// </summary>
    Task<List<Employee>> GetAllForReportAsync(
        string? query, string? sortBy, bool isDescending,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an employee exists.
    /// </summary>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
