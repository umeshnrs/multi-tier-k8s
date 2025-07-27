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
    public async Task Handle_WhenEventExists_SoftDeletesAndReturnsTrue()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var existingEvent = new Event { Id = eventId, IsDeleted = false };
        var command = new DeleteEventCommand(eventId);

        _mockRepository.Setup(r => r.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        Event capturedEvent = null;
        _mockRepository.Setup(r => r.DeleteAsync(eventId))
            .Callback<Guid>(id => 
            {
                existingEvent.IsDeleted = true;
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(eventId), Times.Once);
        Assert.True(existingEvent.IsDeleted);
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

    [Fact]
    public async Task Handle_WhenEventIsAlreadyDeleted_ReturnsFalse()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var existingEvent = new Event { Id = eventId, IsDeleted = true };
        var command = new DeleteEventCommand(eventId);

        _mockRepository.Setup(r => r.GetByIdAsync(eventId))
            .ReturnsAsync((Event)null); // Repository should return null for already deleted events

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}