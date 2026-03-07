using FluentAssertions;
using EmployeeManagement.Shared.Models;

namespace EmployeeManagement.Tests.Shared;

public class PaginatedListTests
{
    [Fact]
    public void Create_Should_Set_Correct_Metadata()
    {
        var items = new List<string> { "A", "B", "C" };
        var result = PaginatedList<string>.Create(items, totalCount: 25, page: 2, pageSize: 10);

        result.Items.Should().HaveCount(3);
        result.Pagination.Page.Should().Be(2);
        result.Pagination.PageSize.Should().Be(10);
        result.Pagination.TotalCount.Should().Be(25);
        result.Pagination.TotalPages.Should().Be(3);
    }

    [Fact]
    public void TotalPages_Should_Round_Up()
    {
        var result = PaginatedList<int>.Create([], totalCount: 21, page: 1, pageSize: 10);
        result.Pagination.TotalPages.Should().Be(3);
    }

    [Fact]
    public void TotalPages_Should_Be_Zero_When_No_Items()
    {
        var result = PaginatedList<int>.Create([], totalCount: 0, page: 1, pageSize: 10);
        result.Pagination.TotalPages.Should().Be(0);
    }
}

public class PaginationQueryTests
{
    [Fact]
    public void Page_Should_Default_To_1()
    {
        new PaginationQuery().Page.Should().Be(1);
    }

    [Fact]
    public void PageSize_Should_Default_To_20()
    {
        new PaginationQuery().PageSize.Should().Be(20);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    public void Page_Should_Clamp_To_Minimum_1(int input, int expected)
    {
        var q = new PaginationQuery { Page = input };
        q.Page.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(200, 100)]
    [InlineData(50, 50)]
    public void PageSize_Should_Clamp_Between_1_And_100(int input, int expected)
    {
        var q = new PaginationQuery { PageSize = input };
        q.PageSize.Should().Be(expected);
    }

    [Theory]
    [InlineData("asc", false)]
    [InlineData("ASC", false)]
    [InlineData("desc", true)]
    [InlineData("DESC", true)]
    public void IsDescending_Should_Parse_Correctly(string direction, bool expected)
    {
        var q = new PaginationQuery { SortDirection = direction };
        q.IsDescending.Should().Be(expected);
    }
}
