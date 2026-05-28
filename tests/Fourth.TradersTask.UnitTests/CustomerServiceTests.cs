using FluentAssertions;
using Fourth.TradersTask.Application.Abstractions;
using Fourth.TradersTask.Application.Models;
using Fourth.TradersTask.Application.Models.Dtos;
using Fourth.TradersTask.Application.Services;
using Fourth.TradersTask.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fourth.TradersTask.UnitTests;

/// <summary>
/// Unit tests for CustomerService.
/// </summary>
public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly Mock<ILogger<CustomerService>> _mockLogger;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _mockLogger = new Mock<ILogger<CustomerService>>();
        _service = new CustomerService(_mockRepository.Object, _mockLogger.Object);
    }

    #region GetCustomersAsync Tests

    [Fact]
    public async Task GetCustomersAsync_WithValidParams_ReturnsPagedResult()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { CustomerId = "CUST1", CompanyName = "Company 1", Orders = new List<Order>() },
            new() { CustomerId = "CUST2", CompanyName = "Company 2", Orders = new List<Order>() }
        };

        _mockRepository
            .Setup(r => r.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerListDto { Customers = customers, TotalCount = 2 });

        var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _service.GetCustomersAsync(paginationParams);

        // Assert
        result.Should().NotBeNull();
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);
        result.Data.Should().HaveCount(2);
        result.Data[0].CustomerId.Should().Be("CUST1");
        result.Data[0].CompanyName.Should().Be("Company 1");

        _mockRepository.Verify(
            r => r.GetCustomersAsync(1, 10, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCustomersAsync_WithSearchTerm_PassesSearchToRepository()
    {
        // Arrange
        var searchTerm = "test";
        var customers = new List<Customer>();

        _mockRepository
            .Setup(r => r.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerListDto { Customers = customers, TotalCount = 0 });

        var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10, CustomerName = searchTerm };

        // Act
        await _service.GetCustomersAsync(paginationParams);

        // Assert
        _mockRepository.Verify(
            r => r.GetCustomersAsync(1, 10, searchTerm, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCustomersAsync_WithPageSize_CalculatesCorrectTotalPages()
    {
        // Arrange
        var customers = new List<Customer> { new() { CustomerId = "C1", CompanyName = "C1" } };

        _mockRepository
            .Setup(r => r.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerListDto { Customers = customers, TotalCount = 25 });

        var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await _service.GetCustomersAsync(paginationParams);

        // Assert
        result.TotalPages.Should().Be(3);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetCustomersAsync_WithOrders_IncludesOrderCount()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { OrderId = 1 },
            new() { OrderId = 2 },
            new() { OrderId = 3 }
        };

        var customers = new List<Customer>
        {
            new() { CustomerId = "C1", CompanyName = "Company 1", Orders = orders }
        };

        _mockRepository
            .Setup(r => r.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CustomerListDto { Customers = customers, TotalCount = 1 });

        var paginationParams = new PaginationParams();

        // Act
        var result = await _service.GetCustomersAsync(paginationParams);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data[0].NumberOfOrders.Should().Be(3);
    }

    #endregion

    #region GetCustomerDetailAsync Tests

    [Fact]
    public async Task GetCustomerDetailAsync_WithValidCustomerId_ReturnsCustomerDetail()
    {
        // Arrange
        var customerId = "ALFKI";
        var orders = new List<Order>
        {
            new()
            {
                OrderId = 1,
                OrderDate = new DateTime(2023, 1, 1),
                OrderDetails = new List<OrderDetail>
                {
                    new() { UnitPrice = 100, Quantity = 2, Discount = 0 },
                    new() { UnitPrice = 50, Quantity = 1, Discount = 0.1f }
                }
            }
        };

        var customer = new Customer
        {
            CustomerId = customerId,
            CompanyName = "Alfreds Futterkiste",
            ContactName = "Maria Anders",
            Orders = orders
        };

        _mockRepository
            .Setup(r => r.GetCustomerWithOrdersAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetCustomerDetailAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.CustomerId.Should().Be(customerId);
        result.CompanyName.Should().Be("Alfreds Futterkiste");
        result.ContactName.Should().Be("Maria Anders");
        result.Orders.Should().HaveCount(1);
        result.Orders[0].OrderId.Should().Be(1);
        result.Orders[0].NumberOfProducts.Should().Be(2);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_WithNonExistentCustomerId_ReturnsNull()
    {
        // Arrange
        var customerId = "NONEXISTENT";

        _mockRepository
            .Setup(r => r.GetCustomerWithOrdersAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _service.GetCustomerDetailAsync(customerId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCustomerDetailAsync_CalculatesOrderTotalCorrectly()
    {
        // Arrange
        var customerId = "CUST1";
        var orders = new List<Order>
        {
            new()
            {
                OrderId = 1,
                OrderDate = DateTime.Now,
                OrderDetails = new List<OrderDetail>
                {
                    // 100 * 2 * (1 - 0) = 200
                    new() { UnitPrice = 100, Quantity = 2, Discount = 0 },
                    // 50 * 1 * (1 - 0.1) = 45
                    new() { UnitPrice = 50, Quantity = 1, Discount = 0.1f }
                }
            }
        };

        var customer = new Customer
        {
            CustomerId = customerId,
            CompanyName = "Test Company",
            Orders = orders
        };

        _mockRepository
            .Setup(r => r.GetCustomerWithOrdersAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetCustomerDetailAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.Orders[0].TotalOrderValue.Should().Be(245m);
    }

    [Fact]
    public async Task GetCustomerDetailAsync_OrdersAreSortedByDateDescending()
    {
        // Arrange
        var customerId = "CUST1";
        var orders = new List<Order>
        {
            new() { OrderId = 1, OrderDate = new DateTime(2023, 1, 1), OrderDetails = new List<OrderDetail>() },
            new() { OrderId = 2, OrderDate = new DateTime(2023, 3, 1), OrderDetails = new List<OrderDetail>() },
            new() { OrderId = 3, OrderDate = new DateTime(2023, 2, 1), OrderDetails = new List<OrderDetail>() }
        };

        var customer = new Customer
        {
            CustomerId = customerId,
            CompanyName = "Test",
            Orders = orders
        };

        _mockRepository
            .Setup(r => r.GetCustomerWithOrdersAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetCustomerDetailAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.Orders.Should().HaveCount(3);
        result.Orders[0].OrderId.Should().Be(2);
        result.Orders[1].OrderId.Should().Be(3);
        result.Orders[2].OrderId.Should().Be(1);
    }

    #endregion
}
