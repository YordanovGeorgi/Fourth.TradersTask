namespace Fourth.TradersTask.Application.Models;

/// <summary>
/// Represents pagination parameters for list requests.
/// </summary>
public class PaginationParams
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    public int PageNumber { get; set; } = 1;
    
    private int _pageSize = DefaultPageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? CustomerName { get; set; }
}
