using EventBooking.API.Features.Events.Queries;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventBooking.Tests.Features.Events.Queries;

public class GetEventByIdQueryHandlerTests
{
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<ILogger<GetEventByIdQueryHandler>> _mockLogger;
    private readonly GetEventByIdQueryHandler _handler;

    public GetEventByIdQueryHandlerTests()
    {
        _mockEventRepository = new Mock<IEventRepository>();
        _mockLogger = new Mock<ILogger<GetEventByIdQueryHandler>>();
        _handler = new GetEventByIdQueryHandler(_mockEventRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ExistingEvent_ReturnsEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var expectedEvent = new Event
        {
            Id = eventId,
            Title = "Test Event",
            Description = "Test Description"
        };

        _mockEventRepository.Setup(x => x.GetByIdAsync(eventId))
            .ReturnsAsync(expectedEvent);

        // Act
        var result = await _handler.Handle(new GetEventByIdQuery(eventId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedEvent);
    }

    [Fact]
    public async Task Handle_NonExistingEvent_ReturnsNull()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        _mockEventRepository.Setup(x => x.GetByIdAsync(eventId))
            .ReturnsAsync((Event?)null);

        // Act
        var result = await _handler.Handle(new GetEventByIdQuery(eventId), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}