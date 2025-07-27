using EventBooking.API.Controllers;
using EventBooking.API.Exceptions;
using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Features.Events.Dtos;
using EventBooking.API.Models;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventBooking.Tests.Controllers;

public class EventsControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<EventsController>> _mockLogger;
    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<EventsController>>();
        _controller = new EventsController(_mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateEvent_ValidCommand_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Test Venue",
            TotalSeats = 100,
            Price = 99.99m
        };

        var createdEvent = new Event
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            VenueName = command.VenueName,
            TotalSeats = command.TotalSeats,
            AvailableSeats = command.TotalSeats,
            Price = command.Price,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid()
        };

        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdEvent);

        // Act
        var actionResult = await _controller.CreateEvent(command);

        // Assert
        var result = actionResult.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var returnValue = result.Value.Should().BeOfType<EventDto>().Subject;

        result.ActionName.Should().Be(nameof(EventsController.GetEvent));
        result.RouteValues!["id"].Should().Be(createdEvent.Id);

        returnValue.Should().NotBeNull();
        returnValue.Title.Should().Be(command.Title);
        returnValue.Description.Should().Be(command.Description);
        returnValue.StartDate.Should().Be(command.StartDate);
        returnValue.EndDate.Should().Be(command.EndDate);
        returnValue.VenueName.Should().Be(command.VenueName);
        returnValue.TotalSeats.Should().Be(command.TotalSeats);
        returnValue.Price.Should().Be(command.Price);
    }

    [Fact]
    public async Task CreateEvent_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Test Venue",
            TotalSeats = 100,
            Price = 99.99m
        };

        var validationMessage = "Validation failed";
        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(validationMessage));

        // Act
        var actionResult = await _controller.CreateEvent(command);

        // Assert
        actionResult.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(validationMessage);

        // Verify logging
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(validationMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task CreateEvent_WhenUnexpectedError_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Test Venue",
            TotalSeats = 100,
            Price = 99.99m
        };

        var errorMessage = "An error occurred while creating the event";
        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var actionResult = await _controller.CreateEvent(command);

        // Assert
        actionResult.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(errorMessage);

        // Verify logging
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
    [Fact]
    public async Task UpdateEvent_WithMatchingIds_ReturnsOkResult()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new UpdateEventCommand { Id = eventId };
        var updatedEvent = new Event { Id = eventId };

        _mockMediator.Setup(m => m.Send(command, default))
            .ReturnsAsync(updatedEvent);

        // Act
        var result = await _controller.UpdateEvent(eventId, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EventDto>(okResult.Value);
        Assert.Equal(eventId, returnValue.Id);
    }

    [Fact]
    public async Task UpdateEvent_WithMismatchedIds_ReturnsBadRequest()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new UpdateEventCommand { Id = Guid.NewGuid() };

        // Act
        var result = await _controller.UpdateEvent(eventId, command);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateEvent_WhenEventNotFound_ReturnsNotFound()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new UpdateEventCommand { Id = eventId };

        _mockMediator.Setup(m => m.Send(command, default))
            .ThrowsAsync(new NotFoundException($"Event with ID {eventId} not found"));

        // Act
        var result = await _controller.UpdateEvent(eventId, command);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateEvent_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var command = new UpdateEventCommand { Id = eventId };

        _mockMediator.Setup(m => m.Send(command, default))
            .ThrowsAsync(new ValidationException("Validation failed"));

        // Act
        var result = await _controller.UpdateEvent(eventId, command);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteEvent_WhenEventExists_ReturnsNoContent()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteEventCommand>(), default))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteEvent_WhenEventNotFound_ReturnsNotFound()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteEventCommand>(), default))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteEvent_WhenExceptionOccurs_ReturnsBadRequest()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteEventCommand>(), default))
            .ThrowsAsync(new Exception("Something went wrong"));

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}