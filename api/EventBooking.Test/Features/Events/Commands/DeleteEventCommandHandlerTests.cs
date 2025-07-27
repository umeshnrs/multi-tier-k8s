using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventBooking.Tests.Features.Events.Commands;

public class DeleteEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _mockRepository;
    private readonly Mock<ILogger<DeleteEventCommandHandler>> _mockLogger;
    private readonly DeleteEventCommandHandler _handler;

    public DeleteEventCommandHandlerTests()
    {
        _mockRepository = new Mock<IEventRepository>();
        _mockLogger = new Mock<ILogger<DeleteEventCommandHandler>>();
        _handler = new DeleteEventCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WhenEventExists_DeletesAndReturnsTrue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var existingEvent = new Event { Id = eventId };
        var command = new DeleteEventCommand(eventId);

        _mockRepository.Setup(r => r.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockRepository.Setup(r => r.DeleteAsync(existingEvent.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(existingEvent.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new DeleteEventCommand(eventId);

        _mockRepository.Setup(r => r.GetByIdAsync(eventId))
            .ReturnsAsync((Event)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}