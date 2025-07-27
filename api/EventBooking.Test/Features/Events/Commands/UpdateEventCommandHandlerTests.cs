using EventBooking.API.Exceptions;
using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventBooking.Tests.Features.Events.Commands;

public class UpdateEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _mockRepository;
    private readonly Mock<ILogger<UpdateEventCommandHandler>> _mockLogger;
    private readonly UpdateEventCommandHandler _handler;
    private readonly UpdateEventCommandValidator _validator;

    public UpdateEventCommandHandlerTests()
    {
        _mockRepository = new Mock<IEventRepository>();
        _mockLogger = new Mock<ILogger<UpdateEventCommandHandler>>();
        _handler = new UpdateEventCommandHandler(_mockRepository.Object, _mockLogger.Object);
        _validator = new UpdateEventCommandValidator();
    }

    [Fact]
    public async Task Handle_WhenEventExists_UpdatesAndReturnsEvent()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new UpdateEventCommand
        {
            Id = eventId,
            Title = "Updated Event",
            Description = "Updated Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Updated Venue",
            TotalSeats = 200,
            AvailableSeats = 150,
            Price = 299.99M
        };

        var existingEvent = new Event
        {
            Id = eventId,
            Title = "Original Event",
            Description = "Original Description",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1),
            VenueName = "Original Venue",
            TotalSeats = 100,
            AvailableSeats = 50,
            Price = 199.99M
        };

        _mockRepository.Setup(r => r.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Event>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.StartDate.ToUniversalTime(), result.StartDate);
        Assert.Equal(command.EndDate.ToUniversalTime(), result.EndDate);
        Assert.Equal(command.VenueName, result.VenueName);
        Assert.Equal(command.TotalSeats, result.TotalSeats);
        Assert.Equal(command.AvailableSeats, result.AvailableSeats);
        Assert.Equal(command.Price, result.Price);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new UpdateEventCommand { Id = eventId };

        _mockRepository.Setup(r => r.GetByIdAsync(eventId))
            .ReturnsAsync((Event)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new UpdateEventCommand
        {
            Id = Guid.NewGuid(),
            Title = "Valid Title",
            Description = "Valid Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Valid Venue",
            TotalSeats = 100,
            AvailableSeats = 50,
            Price = 199.99M
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "Description", "Venue")] // Empty title
    [InlineData("Title", "", "Venue")] // Empty description
    [InlineData("Title", "Description", "")] // Empty venue
    public void Validate_InvalidCommand_FailsValidation(string title, string description, string venue)
    {
        // Arrange
        var command = new UpdateEventCommand
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = venue,
            TotalSeats = 100,
            AvailableSeats = 50,
            Price = 199.99M
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }
}