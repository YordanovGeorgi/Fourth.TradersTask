using System.ComponentModel;

namespace Fourth.TradersTask.Application.Models;
/// <summary>
/// QueryParameters
/// </summary>
public class GetCustomersQueryParameters
{
    /// <summary>
    /// The page number (default 1)
    /// </summary>
    [DefaultValue(1)]
    public int PageNumber { get; set; }
    /// <summary>
    /// The number of items per page (default 10, max 100).
    /// </summary>
    [DefaultValue(10)]
    public int PageSize { get; set; }
    /// <summary>
    /// Optional search term to filter by company name.
    /// </summary>
    public string? CustomerName { get; set; }
}
