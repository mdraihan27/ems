namespace EmployeeManagement.Shared.Models;

/// <summary>
/// Pagination metadata included in list responses.
/// </summary>
public class PaginationMeta
{
    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

/// <summary>
/// Paginated list response containing items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public class PaginatedList<T>
{
    /// <summary>
    /// The items for the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; set; } = [];

    /// <summary>
    /// Pagination metadata.
    /// </summary>
    public PaginationMeta Pagination { get; set; } = new();

    /// <summary>
    /// Creates a new paginated list from a subset of items.
    /// </summary>
    public static PaginatedList<T> Create(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
    {
        return new PaginatedList<T>
        {
            Items = items,
            Pagination = new PaginationMeta
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            }
        };
    }
}
