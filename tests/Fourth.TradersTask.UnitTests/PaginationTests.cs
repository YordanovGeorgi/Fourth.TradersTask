using Xunit;
using FluentAssertions;
using Fourth.TradersTask.Application.Models;

namespace Fourth.TradersTask.UnitTests;

/// <summary>
/// Unit tests for pagination models.
/// </summary>
public class PaginationTests
{
    [Fact]
    public void PaginationParams_DefaultPageNumber_IsOne()
    {
        // Act
        var pagination = new PaginationParams();

        // Assert
        pagination.PageNumber.Should().Be(1);
    }

    [Fact]
    public void PaginationParams_DefaultPageSize_IsTen()
    {
        // Act
        var pagination = new PaginationParams();

        // Assert
        pagination.PageSize.Should().Be(10);
    }

    [Fact]
    public void PaginationParams_PageSizeExceedsMax_IsLimitedToMax()
    {
        // Act
        var pagination = new PaginationParams { PageSize = 150 };

        // Assert
        pagination.PageSize.Should().Be(100);
    }

    [Fact]
    public void PaginationParams_PageSizeIsValid_Unchanged()
    {
        // Act
        var pagination = new PaginationParams { PageSize = 50 };

        // Assert
        pagination.PageSize.Should().Be(50);
    }

    [Fact]
    public void PaginationParams_SearchTermCanBeNull()
    {
        // Act
        var pagination = new PaginationParams { CustomerName = null };

        // Assert
        pagination.CustomerName.Should().BeNull();
    }

    [Fact]
    public void PagedResult_CalculatesTotalPages_Correctly()
    {
        // Arrange
        var items = new List<string> { "1", "2", "3" };

        // Act
        var result = new PagedResult<string>(1, 10, 25, items);

        // Assert
        result.TotalPages.Should().Be(3); // ceil(25 / 10)
    }

    [Fact]
    public void PagedResult_FirstPage_HasNoPreviousPage()
    {
        // Arrange
        var items = new List<string>();

        // Act
        var result = new PagedResult<string>(1, 10, 100, items);

        // Assert
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void PagedResult_LastPage_HasNoNextPage()
    {
        // Arrange
        var items = new List<string>();

        // Act
        var result = new PagedResult<string>(3, 10, 25, items);

        // Assert
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void PagedResult_MiddlePage_HasBothPreviousAndNext()
    {
        // Arrange
        var items = new List<string>();

        // Act
        var result = new PagedResult<string>(2, 10, 100, items);

        // Assert
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void PagedResult_SinglePage_HasNoNavigation()
    {
        // Arrange
        var items = new List<string> { "1", "2" };

        // Act
        var result = new PagedResult<string>(1, 10, 5, items);

        // Assert
        result.TotalPages.Should().Be(1);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }
}
