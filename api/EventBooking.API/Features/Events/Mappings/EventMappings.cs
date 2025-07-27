using EventBooking.API.Features.Events.Dtos;
using EventBooking.API.Models;

namespace EventBooking.API.Features.Events.Mappings;

public static class EventMappings
{
    public static EventDto ToDto(this Event @event)
    {
        return new EventDto
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            VenueName = @event.VenueName,
            TotalSeats = @event.TotalSeats,
            AvailableSeats = @event.AvailableSeats,
            Price = @event.Price,
            CreatedAt = @event.CreatedAt
        };
    }
}