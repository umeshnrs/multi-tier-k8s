using MediatR;
using EventBooking.API.Models;
using EventBooking.API.Interfaces;

namespace EventBooking.API.Features.Events.Commands;

public record CreateEventCommand : IRequest<Event>
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string VenueName { get; init; } = string.Empty;
    public int TotalSeats { get; init; }
    public decimal Price { get; init; }
}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Event>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<CreateEventCommandHandler> _logger;

    public CreateEventCommandHandler(
        IEventRepository eventRepository,
        ILogger<CreateEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<Event> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new Event
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                VenueName = request.VenueName,
                TotalSeats = request.TotalSeats,
                AvailableSeats = request.TotalSeats,
                Price = request.Price,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid() // This should come from authenticated user context
            };

            var result = await _eventRepository.AddAsync(@event);
            _logger.LogInformation("Event created successfully: {EventId}", result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            throw;
        }
    }
}