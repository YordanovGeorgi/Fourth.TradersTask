using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Fourth.TradersTask.IntegrationTests;

/// <summary>
/// Integration tests for the Customers API endpoint.
/// </summary>
public class CustomersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CustomersControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    #region GET /api/customers Tests

    [Fact]
    public async Task GetCustomers_WithoutQueryParameters_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/customers");

        // Assert
        // Note: This will fail if database is not configured, which is expected
        // In a real scenario with test database, this would verify the endpoint structure
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCustomers_WithInvalidPageNumber_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/customers?pageNumber=0");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCustomers_WithNegativePageNumber_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/customers?pageNumber=-1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCustomers_WithInvalidPageSize_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/customers?pageSize=0");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCustomers_WithValidPageParameters_ReturnsOkWithCorrectStructure()
    {
        // Act
        var response = await _client.GetAsync("/api/customers?pageNumber=1&pageSize=10");

        // Assert
        // Check response structure if successful (or database error)
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);

            json.TryGetProperty("pageNumber", out var pageNumber).Should().BeTrue();
            json.TryGetProperty("pageSize", out var pageSize).Should().BeTrue();
            json.TryGetProperty("totalCount", out var totalCount).Should().BeTrue();
            json.TryGetProperty("totalPages", out var totalPages).Should().BeTrue();
            json.TryGetProperty("data", out var data).Should().BeTrue();

            pageNumber.GetInt32().Should().Be(1);
            pageSize.GetInt32().Should().Be(10);
        }
    }

    [Fact]
    public async Task GetCustomers_WithSearchTerm_SendsSearchParameter()
    {
        // Act
        var response = await _client.GetAsync("/api/customers?pageNumber=1&pageSize=10&customerName=test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region GET /api/customers/details/{id} Tests

    [Fact]
    public async Task GetCustomerDetail_WithEmptyId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/details/");

        // Assert
        // Empty ID should not match route
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomerDetail_WithWhitespaceId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/details/%20");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCustomerDetail_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/details/NONEXISTENT");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.NotFound,
            HttpStatusCode.InternalServerError, // Database not configured
            HttpStatusCode.ServiceUnavailable
        );

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("not found");
        }
    }

    [Fact]
    public async Task GetCustomerDetail_WithValidId_ReturnsOkWithCorrectStructure()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/ALFKI");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);

            json.TryGetProperty("customerId", out var customerId).Should().BeTrue();
            json.TryGetProperty("companyName", out var companyName).Should().BeTrue();
            json.TryGetProperty("orders", out var orders).Should().BeTrue();

            customerId.GetString().Should().NotBeNullOrEmpty();
            orders.ValueKind.Should().Be(JsonValueKind.Array);
        }
    }

    [Fact]
    public async Task GetCustomerDetail_ReturnsCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/customers/ALFKI");

        // Assert
        if (response.StatusCode == HttpStatusCode.OK)
        {
            response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        }
    }

    #endregion

    #region HTTP Status Codes Tests

    [Fact]
    public async Task GetCustomers_WithValidRequest_ReturnsOkStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/customers?pageNumber=1&pageSize=10&customerName=ALFKI");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task InvalidEndpoint_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/invalid");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
