using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventBooking.Test.Features.Events.Commands;
public class CreateEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<ILogger<CreateEventCommandHandler>> _mockLogger;
    private readonly CreateEventCommandHandler _handler;
    private readonly CreateEventCommandValidator _validator;

    public CreateEventCommandHandlerTests()
    {
        _mockEventRepository = new Mock<IEventRepository>();
        _mockLogger = new Mock<ILogger<CreateEventCommandHandler>>();
        _handler = new CreateEventCommandHandler(_mockEventRepository.Object, _mockLogger.Object);
        _validator = new CreateEventCommandValidator();
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesEvent()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            Title = "Tech Summit 2024",
            Description = "Annual Technology Summit",
            StartDate = DateTime.UtcNow.AddDays(30),
            EndDate = DateTime.UtcNow.AddDays(32),
            VenueName = "Convention Center",
            TotalSeats = 1000,
            Price = 299.99m
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

        _mockEventRepository.Setup(x => x.AddAsync(It.IsAny<Event>()))
            .ReturnsAsync(createdEvent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createdEvent, options =>
            options.ComparingByMembers<Event>());

        _mockEventRepository.Verify(x => x.AddAsync(It.Is<Event>(e =>
            e.Title == command.Title &&
            e.Description == command.Description &&
            e.StartDate == command.StartDate &&
            e.EndDate == command.EndDate &&
            e.VenueName == command.VenueName &&
            e.TotalSeats == command.TotalSeats &&
            e.AvailableSeats == command.TotalSeats &&
            e.Price == command.Price)), Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Event created successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryFails_ThrowsException()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            Title = "Tech Summit 2024",
            Description = "Annual Technology Summit",
            StartDate = DateTime.UtcNow.AddDays(30),
            EndDate = DateTime.UtcNow.AddDays(32),
            VenueName = "Convention Center",
            TotalSeats = 1000,
            Price = 299.99m
        };

        var expectedException = new Exception("Database error");
        _mockEventRepository.Setup(x => x.AddAsync(It.IsAny<Event>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Should().Be(expectedException);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

public class CreateEventCommandValidatorTests
{
    private readonly CreateEventCommandValidator _validator;

    public CreateEventCommandValidatorTests()
    {
        _validator = new CreateEventCommandValidator();
    }

    [Fact]
    public async Task Validate_StartDateInPast_HasCustomError()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            StartDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        var error = result.Errors.Should().ContainSingle(x => x.PropertyName == "StartDate").Subject;
        error.ErrorMessage.Should().Contain("must be greater than");
    }

    [Fact]
    public async Task Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new CreateEventCommand
        {
            Title = "Valid Event",
            Description = "Valid Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Valid Venue",
            TotalSeats = 100,
            Price = 99.99m
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
