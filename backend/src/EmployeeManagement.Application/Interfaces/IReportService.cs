using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces;

/// <summary>
/// Service interface for PDF report generation.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generates a PDF table report of employees matching the optional filter.
    /// </summary>
    byte[] GenerateEmployeeListReport(List<Employee> employees);

    /// <summary>
    /// Generates a PDF CV report for a single employee including family details.
    /// </summary>
    byte[] GenerateEmployeeCvReport(Employee employee);
}
