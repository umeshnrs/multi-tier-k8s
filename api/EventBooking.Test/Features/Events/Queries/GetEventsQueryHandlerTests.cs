using EventBooking.API.Features.Events.Queries;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using FluentAssertions;
using Moq;

namespace EventBooking.Test.Features.Events.Queries;

public class GetEventsQueryHandlerTests
{
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly GetEventsQueryHandler _handler;
    private readonly List<Event> _testEvents;

    public GetEventsQueryHandlerTests()
    {
        _mockEventRepository = new Mock<IEventRepository>();
        _handler = new GetEventsQueryHandler(_mockEventRepository.Object);

        // Setup test data
        _testEvents = new List<Event>
        {
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Web Development Summit",
                Description = "Learn about web development",
                VenueName = "Tech Center",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                TotalSeats = 100,
                AvailableSeats = 50,
                Price = 99.99M,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "Mobile App Workshop",
                Description = "Mobile development workshop",
                VenueName = "Innovation Hub",
                StartDate = DateTime.UtcNow.AddDays(3),
                EndDate = DateTime.UtcNow.AddDays(4),
                TotalSeats = 50,
                AvailableSeats = 25,
                Price = 149.99M,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Title = "DevOps Conference",
                Description = "Learn about DevOps practices",
                VenueName = "Tech Center",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(6),
                TotalSeats = 200,
                AvailableSeats = 100,
                Price = 299.99M,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };
    }

    [Fact]
    public async Task Handle_WithoutSearchAndPagination_ReturnsAllEvents()
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalPages.Should().Be(1);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }

    [Theory]
    [InlineData("web", 1)] // Should find "Web Development Summit"
    [InlineData("mobile", 1)] // Should find "Mobile App Workshop"
    [InlineData("tech center", 2)] // Should find events at "Tech Center"
    [InlineData("nonexistent", 0)] // Should find no events
    public async Task Handle_WithSearch_ReturnsFilteredEvents(string searchTerm, int expectedCount)
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            SearchTerm = searchTerm,
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(expectedCount);
        result.TotalCount.Should().Be(expectedCount);
    }

    [Theory]
    [InlineData(1, 2, 2)] // First page of 2 items
    [InlineData(2, 2, 1)] // Second page of 2 items
    [InlineData(1, 3, 3)] // All items in one page
    [InlineData(2, 3, 0)] // Empty page
    public async Task Handle_WithPagination_ReturnsCorrectPage(int pageNumber, int pageSize, int expectedCount)
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(expectedCount);
        result.PageNumber.Should().Be(pageNumber);
        result.PageSize.Should().Be(pageSize);
        result.TotalCount.Should().Be(_testEvents.Count);
        result.TotalPages.Should().Be((int)Math.Ceiling(_testEvents.Count / (double)pageSize));
    }

    [Fact]
    public async Task Handle_WithSearchAndPagination_ReturnsCorrectResults()
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            SearchTerm = "tech",
            PageNumber = 1,
            PageSize = 1
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(2); // Two events have "Tech" in their venue
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(1);
        result.TotalPages.Should().Be(2);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 10)] // Invalid page number
    [InlineData(1, 0)]  // Invalid page size
    [InlineData(1, 101)] // Page size exceeds maximum
    public async Task Handle_WithInvalidPagination_ThrowsArgumentException(int pageNumber, int pageSize)
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(query, CancellationToken.None));

        if (pageNumber < 1)
            Assert.Equal("Page number must be greater than 0 (Parameter 'PageNumber')", exception.Message);
        else if (pageSize < 1)
            Assert.Equal("Page size must be greater than 0 (Parameter 'PageSize')", exception.Message);
        else if (pageSize > 100)
            Assert.Equal("Page size cannot exceed 100 (Parameter 'PageSize')", exception.Message);
    }

    [Theory]
    [InlineData(OrderByField.StartDate, OrderDirection.Ascending)]
    [InlineData(OrderByField.StartDate, OrderDirection.Descending)]
    [InlineData(OrderByField.CreatedAt, OrderDirection.Ascending)]
    [InlineData(OrderByField.CreatedAt, OrderDirection.Descending)]
    public async Task Handle_WithOrdering_ReturnsCorrectlyOrderedEvents(OrderByField orderBy, OrderDirection orderDirection)
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = orderBy,
            OrderDirection = orderDirection
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);

        var items = result.Items.ToList();
        
        if (orderBy == OrderByField.StartDate)
        {
            if (orderDirection == OrderDirection.Ascending)
            {
                items.Should().BeInAscendingOrder(e => e.StartDate);
                items[0].Title.Should().Be("Web Development Summit");
                items[2].Title.Should().Be("DevOps Conference");
            }
            else
            {
                items.Should().BeInDescendingOrder(e => e.StartDate);
                items[0].Title.Should().Be("DevOps Conference");
                items[2].Title.Should().Be("Web Development Summit");
            }
        }
        else // OrderByField.CreatedAt
        {
            if (orderDirection == OrderDirection.Ascending)
            {
                items.Should().BeInAscendingOrder(e => e.CreatedAt);
                items[0].Title.Should().Be("Web Development Summit");
                items[2].Title.Should().Be("DevOps Conference");
            }
            else
            {
                items.Should().BeInDescendingOrder(e => e.CreatedAt);
                items[0].Title.Should().Be("DevOps Conference");
                items[2].Title.Should().Be("Web Development Summit");
            }
        }
    }

    [Fact]
    public async Task Handle_WithDefaultOrdering_ReturnsStartDateAscending()
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.Items.Should().BeInAscendingOrder(e => e.StartDate);
        result.Items.First().Title.Should().Be("Web Development Summit");
        result.Items.Last().Title.Should().Be("DevOps Conference");
    }

    [Fact]
    public async Task Handle_WithSearchAndOrdering_ReturnsCorrectlyFilteredAndOrderedEvents()
    {
        // Arrange
        _mockEventRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(_testEvents);

        var query = new GetEventsQuery
        {
            SearchTerm = "tech",
            PageNumber = 1,
            PageSize = 10,
            OrderBy = OrderByField.StartDate,
            OrderDirection = OrderDirection.Descending
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2); // Two events with "Tech" in venue
        result.Items.Should().BeInDescendingOrder(e => e.StartDate);
        result.Items.First().VenueName.Should().Be("Tech Center");
        result.Items.Last().VenueName.Should().Be("Tech Center");
    }
}