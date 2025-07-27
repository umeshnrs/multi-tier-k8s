using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Models;

namespace EventBooking.Test.Helpers;

public static class TestDataHelper
{
    public static CreateEventCommand CreateValidCommand()
    {
        return new CreateEventCommand
        {
            Title = "Test Event",
            Description = "Test Description",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            VenueName = "Test Venue",
            TotalSeats = 100,
            Price = 99.99m
        };
    }

    public static Event CreateTestEvent(CreateEventCommand command)
    {
        return new Event
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
    }
}