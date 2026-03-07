namespace EmployeeManagement.Shared.Models;

/// <summary>
/// Query parameters for paginated and sorted list requests.
/// </summary>
public class PaginationQuery
{
    private int _page = 1;
    private int _pageSize = 20;

    /// <summary>
    /// Page number (1-based, default: 1).
    /// </summary>
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Number of items per page (default: 20, max: 100).
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : (value > 100 ? 100 : value);
    }

    /// <summary>
    /// Field to sort by (Name, Department, NID, BasicSalary).
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc" (default: "asc").
    /// </summary>
    public string SortDirection { get; set; } = "asc";

    /// <summary>
    /// Whether sort direction is descending.
    /// </summary>
    public bool IsDescending => SortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);
}
