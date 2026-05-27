using Fourth.TradersTask.Application.Models;
using Fourth.TradersTask.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fourth.TradersTask.API.Controllers;

/// <summary>
/// API controller for customer operations.
/// </summary>
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a paginated list of customers with optional search filtering.
    /// </summary>
    /// <param name="pageNumber">The page number (default 1).</param>
    /// <param name="pageSize">The number of items per page (default 10, max 100).</param>
    /// <param name="customerName">Optional search term to filter by company name or contact name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of customers.</returns>
    /// <response code="200">Returns the paginated list of customers.</response>
    /// <response code="400">If pagination parameters are invalid.</response>
    [HttpGet]
    public async Task<ActionResult<PagedResult<CustomerListItemDto>>> GetCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? customerName = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GET /api/customers - Page: {PageNumber}, Size: {PageSize}, Search: {CustomerName}",
            pageNumber, pageSize, customerName ?? "none");

        // Validation
        if (pageNumber < 1)
        {
            _logger.LogWarning("Invalid pageNumber: {PageNumber}", pageNumber);
            return BadRequest(new { message = "Page number must be greater than 0." });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("Invalid pageSize: {PageSize}", pageSize);
            return BadRequest(new { message = "Page size must be between 1 and 100." });
        }

        var paginationParams = new PaginationParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            CustomerName = customerName
        };

        var result = await _customerService.GetCustomersAsync(paginationParams, cancellationToken);

        if (result.TotalCount == 0)
        {
            _logger.LogWarning("No customers found for the given parameters: {PaginationParams}", paginationParams);
            return NotFound(new { message = "No customers found for the given parameters." });
        }

        _logger.LogInformation("Successfully retrieved {Count} customers out of {Total}",
            result.Data.Count, result.TotalCount);

        return Ok(result);
    }

    /// <summary>
    /// Gets detailed information about a specific customer including order summaries.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Customer details with order summaries.</returns>
    /// <response code="200">Returns the customer details.</response>
    /// <response code="404">If the customer is not found.</response>
    [HttpGet("details/{id}")]
    public async Task<ActionResult<CustomerDetailDto>> GetCustomerDetail(
        string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GET /api/customers/details/{CustomerId}", id);

        if (string.IsNullOrWhiteSpace(id))
        {
            _logger.LogWarning("Customer ID is null or empty");
            return BadRequest(new { message = "Customer ID cannot be empty." });
        }

        var customer = await _customerService.GetCustomerDetailAsync(id.Trim(), cancellationToken);

        if (customer is null)
        {
            _logger.LogWarning("Customer not found: {CustomerId}", id);
            return NotFound(new { message = $"Customer with ID '{id}' not found." });
        }

        _logger.LogInformation("Successfully retrieved customer details for {CustomerId}", id);

        return Ok(customer);
    }
}
