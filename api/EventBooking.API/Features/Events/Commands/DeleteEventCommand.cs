using EventBooking.API.Interfaces;
using MediatR;

namespace EventBooking.API.Features.Events.Commands;

public record DeleteEventCommand(Guid Id) : IRequest<bool>;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<DeleteEventCommandHandler> _logger;

    public DeleteEventCommandHandler(
        IEventRepository eventRepository,
        ILogger<DeleteEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.GetByIdAsync(request.Id);
        if (existingEvent == null)
        {
            _logger.LogWarning("Attempted to delete non-existent event: {EventId}", request.Id);
            return false;
        }

        await _eventRepository.DeleteAsync(existingEvent.Id);

        _logger.LogInformation("Event deleted successfully: {EventId}", request.Id);
        return true;
    }
}