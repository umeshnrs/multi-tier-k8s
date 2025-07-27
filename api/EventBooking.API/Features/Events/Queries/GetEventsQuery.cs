using EventBooking.API.Features.Events.Dtos;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using MediatR;

namespace EventBooking.API.Features.Events.Queries;

public record GetEventsQuery : IRequest<PagedResponse<Event>>
{
    public string? SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, PagedResponse<Event>>
{
    private readonly IEventRepository _eventRepository;
    private const int MaxPageSize = 100;

    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<PagedResponse<Event>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0", nameof(request.PageNumber));

        if (request.PageSize < 1)
            throw new ArgumentException("Page size must be greater than 0", nameof(request.PageSize));

        if (request.PageSize > MaxPageSize)
            throw new ArgumentException($"Page size cannot exceed {MaxPageSize}", nameof(request.PageSize));

        var query = await _eventRepository.GetAllAsync();

        // Apply search if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(e =>
                e.Title.ToLower().Contains(searchTerm) ||
                e.Description.ToLower().Contains(searchTerm) ||
                e.VenueName.ToLower().Contains(searchTerm)
            );
        }

        // Get total count before pagination
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        // Apply pagination
        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResponse<Event>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages,
            TotalCount = totalCount
        };
    }
}