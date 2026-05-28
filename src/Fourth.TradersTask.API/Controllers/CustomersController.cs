using Fourth.TradersTask.API.Constants;
using Fourth.TradersTask.API.Validators;
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
    private readonly PaginationParamsValidator _paginationValidator;
    private readonly CustomerIdValidator _customerIdValidator;

    /// <summary>
    /// Initializes a new instance of the CustomersController.
    /// </summary>
    public CustomersController(
        ICustomerService customerService,
        ILogger<CustomersController> logger,
        PaginationParamsValidator paginationValidator,
        CustomerIdValidator customerIdValidator)
    {
        _customerService = customerService;
        _logger = logger;
        _paginationValidator = paginationValidator;
        _customerIdValidator = customerIdValidator;
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
    [Produces("application/json")]
    public async Task<ActionResult<PagedResult<CustomerListItemDto>>> GetCustomers(
        [FromQuery] int pageNumber = ApiConstants.DefaultPageNumber,
        [FromQuery] int pageSize = ApiConstants.DefaultPageSize,
        [FromQuery] string? customerName = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "GET /api/customers - Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm}",
            pageNumber, pageSize, customerName ?? ApiConstants.CustomerName);

        // Validate pagination parameters
        var paginationParams = new PaginationParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            CustomerName = customerName
        };

        var validationResult = await _paginationValidator.ValidateAsync(paginationParams, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Invalid pagination parameters: PageNumber={PageNumber}, PageSize={PageSize}",
                pageNumber, pageSize);

            return BadRequest(new ErrorResponse(
                "One or more validation errors occurred.",
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                "VALIDATION_ERROR"));
        }

        var result = await _customerService.GetCustomersAsync(paginationParams, cancellationToken);

        _logger.LogInformation(
            "Successfully retrieved {Count} customers out of {Total}",
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
    /// <response code="400">If the customer ID is invalid.</response>
    /// <response code="404">If the customer is not found.</response>
    [HttpGet("details/{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<CustomerDetailDto>> GetCustomerDetail(
        string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GET /api/customers/details/{CustomerId}", id);

        // Validate customer ID
        var validationResult = await _customerIdValidator.ValidateAsync(id ?? string.Empty, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid customer ID");
            return BadRequest(new ErrorResponse(
                ApiConstants.EmptyCustomerIdMessage,
                null,
                "INVALID_CUSTOMER_ID"));
        }

        var customer = await _customerService.GetCustomerDetailAsync(id.Trim(), cancellationToken);

        if (customer is null)
        {
            var customerId = id.Trim();
            _logger.LogWarning("Customer not found: {CustomerId}", customerId);
            return NotFound(new ErrorResponse(
                string.Format(ApiConstants.CustomerNotFoundMessage, customerId),
                null,
                "CUSTOMER_NOT_FOUND"));
        }

        _logger.LogInformation("Successfully retrieved customer details for {CustomerId}", id);

        return Ok(customer);
    }
}
