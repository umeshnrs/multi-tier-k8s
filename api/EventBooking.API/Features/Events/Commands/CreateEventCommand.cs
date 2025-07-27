using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using FluentValidation;
using MediatR;

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
                CreatedBy = Guid.Parse("2dff81ac-7eb5-40d6-b661-1b8734295043") // This should come from authenticated user context
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

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.VenueName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.TotalSeats)
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
    }
}