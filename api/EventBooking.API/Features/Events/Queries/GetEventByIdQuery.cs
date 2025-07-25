using MediatR;
using EventBooking.API.Models;
using EventBooking.API.Interfaces;

namespace EventBooking.API.Features.Events.Queries;

public record GetEventByIdQuery(Guid Id) : IRequest<Event?>;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, Event?>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<GetEventByIdQueryHandler> _logger;

    public GetEventByIdQueryHandler(
        IEventRepository eventRepository,
        ILogger<GetEventByIdQueryHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<Event?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var @event = await _eventRepository.GetByIdAsync(request.Id);
            
            if (@event == null)
            {
                _logger.LogWarning("Event not found: {EventId}", request.Id);
                return null;
            }

            _logger.LogInformation("Event retrieved successfully: {EventId}", request.Id);
            return @event;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event: {EventId}", request.Id);
            throw;
        }
    }
}