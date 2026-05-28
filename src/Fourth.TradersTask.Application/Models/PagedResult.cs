namespace Fourth.TradersTask.Application.Models;

/// <summary>
/// Generic response object for paginated results.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public class PagedResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Data { get; set; } = new();

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Creates a new instance of PagedResult.
    /// </summary>
    public PagedResult() { }

    /// <summary>
    /// Creates a new instance of PagedResult with parameters.
    /// </summary>
    public PagedResult(int pageNumber, int pageSize, int totalCount, List<T> data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Data = data;
    }
}
