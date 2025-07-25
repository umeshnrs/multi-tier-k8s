using EventBooking.API.Models;

namespace EventBooking.API.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<bool> UpdateAvailableSeatsAsync(Guid eventId, int seats);
}