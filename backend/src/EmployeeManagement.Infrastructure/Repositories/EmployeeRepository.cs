using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Shared.Exceptions;

namespace EmployeeManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Employee data access operations.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the database context.
    /// </summary>
    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .Include(e => e.Spouse)
            .Include(e => e.Children)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Employee?> GetByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .AsNoTracking()
            .Include(e => e.Spouse)
            .Include(e => e.Children)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<(List<Employee> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, string? sortBy, bool isDescending,
        CancellationToken cancellationToken)
    {
        var query = _context.Employees
            .AsNoTracking()
            .Include(e => e.Spouse)
            .Include(e => e.Children)
            .AsQueryable();

        query = ApplySorting(query, sortBy, isDescending);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc />
    public async Task<(List<Employee> Items, int TotalCount)> SearchAsync(
        string searchQuery, int page, int pageSize, string? sortBy, bool isDescending,
        CancellationToken cancellationToken)
    {
        var query = _context.Employees
            .AsNoTracking()
            .Include(e => e.Spouse)
            .Include(e => e.Children)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var search = searchQuery.ToLower();
            query = query.Where(e =>
                EF.Functions.ILike(e.Name, $"%{search}%") ||
                EF.Functions.ILike(e.NID, $"%{search}%") ||
                EF.Functions.ILike(e.Department, $"%{search}%") ||
                EF.Functions.ILike(e.BasicSalary.ToString(), $"%{search}%"));
        }

        query = ApplySorting(query, sortBy, isDescending);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc />
    public async Task<bool> NidExistsAsync(string nid, Guid? excludeEmployeeId, CancellationToken cancellationToken)
    {
        var query = _context.Employees.Where(e => e.NID == nid);
        if (excludeEmployeeId.HasValue)
            query = query.Where(e => e.Id != excludeEmployeeId.Value);
        return await query.AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return employee;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException(
                "The employee record was modified by another user. Please reload and try again.");
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Spouse?> GetSpouseByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        return await _context.Spouses
            .FirstOrDefaultAsync(s => s.EmployeeId == employeeId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Spouse> AddSpouseAsync(Spouse spouse, CancellationToken cancellationToken)
    {
        _context.Spouses.Add(spouse);
        await _context.SaveChangesAsync(cancellationToken);
        return spouse;
    }

    /// <inheritdoc />
    public async Task UpdateSpouseAsync(Spouse spouse, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException(
                "The spouse record was modified by another user. Please reload and try again.");
        }
    }

    /// <inheritdoc />
    public async Task DeleteSpouseAsync(Spouse spouse, CancellationToken cancellationToken)
    {
        _context.Spouses.Remove(spouse);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Child?> GetChildByIdAsync(Guid employeeId, Guid childId, CancellationToken cancellationToken)
    {
        return await _context.Children
            .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.Id == childId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Child> AddChildAsync(Child child, CancellationToken cancellationToken)
    {
        _context.Children.Add(child);
        await _context.SaveChangesAsync(cancellationToken);
        return child;
    }

    /// <inheritdoc />
    public async Task UpdateChildAsync(Child child, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException(
                "The child record was modified by another user. Please reload and try again.");
        }
    }

    /// <inheritdoc />
    public async Task DeleteChildAsync(Child child, CancellationToken cancellationToken)
    {
        _context.Children.Remove(child);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Employee>> GetAllForReportAsync(
        string? query, string? sortBy, bool isDescending,
        CancellationToken cancellationToken)
    {
        var dbQuery = _context.Employees
            .AsNoTracking()
            .Include(e => e.Spouse)
            .Include(e => e.Children)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var search = query.ToLower();
            dbQuery = dbQuery.Where(e =>
                EF.Functions.ILike(e.Name, $"%{search}%") ||
                EF.Functions.ILike(e.NID, $"%{search}%") ||
                EF.Functions.ILike(e.Department, $"%{search}%") ||
                EF.Functions.ILike(e.BasicSalary.ToString(), $"%{search}%"));
        }

        dbQuery = ApplySorting(dbQuery, sortBy, isDescending);
        return await dbQuery.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id, cancellationToken);
    }

    /// <summary>
    /// Applies sorting to the query based on the specified field and direction.
    /// </summary>
    private static IQueryable<Employee> ApplySorting(
        IQueryable<Employee> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
            "department" => isDescending ? query.OrderByDescending(e => e.Department) : query.OrderBy(e => e.Department),
            "nid" => isDescending ? query.OrderByDescending(e => e.NID) : query.OrderBy(e => e.NID),
            "basicsalary" => isDescending ? query.OrderByDescending(e => e.BasicSalary) : query.OrderBy(e => e.BasicSalary),
            _ => query.OrderBy(e => e.Name)
        };
    }
}
