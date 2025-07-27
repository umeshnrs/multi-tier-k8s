using EventBooking.API.Exceptions;
using EventBooking.API.Interfaces;
using EventBooking.API.Models;
using FluentValidation;
using MediatR;

namespace EventBooking.API.Features.Events.Commands;

public record UpdateEventCommand : IRequest<Event>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string VenueName { get; init; } = string.Empty;
    public int TotalSeats { get; init; }
    public int AvailableSeats { get; init; }
    public decimal Price { get; init; }
}

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.StartDate).NotEmpty().GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        RuleFor(x => x.VenueName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TotalSeats).GreaterThan(0);
        RuleFor(x => x.AvailableSeats).GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.TotalSeats);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, Event>
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<UpdateEventCommandHandler> _logger;

    public UpdateEventCommandHandler(
        IEventRepository eventRepository,
        ILogger<UpdateEventCommandHandler> logger)
    {
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<Event> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.GetByIdAsync(request.Id);
        if (existingEvent == null)
        {
            throw new NotFoundException($"Event with ID {request.Id} not found");
        }

        existingEvent.Title = request.Title;
        existingEvent.Description = request.Description;
        existingEvent.StartDate = request.StartDate.ToUniversalTime();
        existingEvent.EndDate = request.EndDate.ToUniversalTime();
        existingEvent.VenueName = request.VenueName;
        existingEvent.TotalSeats = request.TotalSeats;
        existingEvent.AvailableSeats = request.AvailableSeats;
        existingEvent.Price = request.Price;

        await _eventRepository.UpdateAsync(existingEvent);

        _logger.LogInformation("Event updated successfully: {EventId}", request.Id);
        return existingEvent;
    }
}