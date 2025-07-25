using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using MediatR;

namespace EventBooking.API.Features.Events.Queries;

public record GetEventsQuery : IRequest<IEnumerable<Event>>;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, IEnumerable<Event>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<Event>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        return await _eventRepository.GetAllAsync();
    }
}