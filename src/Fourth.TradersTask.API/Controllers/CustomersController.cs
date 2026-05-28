using Fourth.TradersTask.API.Constants;
using Fourth.TradersTask.Application.Abstractions;
using Fourth.TradersTask.Application.Models;
using Fourth.TradersTask.Application.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

    /// <summary>
    /// Initializes a new instance of the CustomersController.
    /// </summary>
    public CustomersController(
        ICustomerService customerService,
        ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    /// Gets a paginated list of customers with optional search filtering.
    /// </summary>
    /// <param name="queryParameters">The query parameters for pagination and filtering.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of customers.</returns>
    /// <response code="200">Returns the paginated list of customers.</response>
    /// <response code="400">If pagination parameters are invalid.</response>
    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<PagedResult<CustomerListItemDto>>> GetCustomers(
        [FromQuery] GetCustomersQueryParameters queryParameters,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "GET /api/customers - Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm}",
            queryParameters.PageNumber, queryParameters.PageSize, queryParameters.CustomerName ?? ApiConstants.CustomerName);

        var paginationParams = new PaginationParams
        {
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            CustomerName = queryParameters.CustomerName
        };

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
        [Required] string id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GET /api/customers/details/{CustomerId}", id);

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

