using MediatR;
using Microsoft.AspNetCore.Mvc;
using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Features.Events.Queries;
using EventBooking.API.Features.Events.Dtos;
using EventBooking.API.Features.Events.Mappings;
using FluentValidation;

namespace EventBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IMediator mediator, ILogger<EventsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
    {
        var events = await _mediator.Send(new GetEventsQuery());
        return Ok(events.Select(e => e.ToDto()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(Guid id)
    {
        var @event = await _mediator.Send(new GetEventByIdQuery(id));
        if (@event == null)
            return NotFound();
            
        return Ok(@event.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventCommand command)
    {
        try
        {
            var @event = await _mediator.Send(command);
            var eventDto = @event.ToDto();
            return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, eventDto);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return BadRequest("An error occurred while creating the event"); // Explicitly using BadRequest
        }
    }
}