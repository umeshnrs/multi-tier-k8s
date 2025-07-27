using EventBooking.API.Exceptions;
using EventBooking.API.Features.Events.Commands;
using EventBooking.API.Features.Events.Dtos;
using EventBooking.API.Features.Events.Mappings;
using EventBooking.API.Features.Events.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<PagedResponse<EventDto>>> GetEvents(
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
            return BadRequest("Page number must be greater than 0");
        if (pageSize < 1)
            return BadRequest("Page size must be greater than 0");
        if (pageSize > 100)
            return BadRequest("Page size cannot exceed 100");

        var query = new GetEventsQuery
        {
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return Ok(new PagedResponse<EventDto>
        {
            Items = result.Items.Select(e => e.ToDto()),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages,
            TotalCount = result.TotalCount
        });
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
            return BadRequest("An error occurred while creating the event");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(Guid id, UpdateEventCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in URL must match ID in request body");
        }

        try
        {
            var @event = await _mediator.Send(command);
            return Ok(@event.ToDto());
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event");
            return BadRequest("An error occurred while updating the event");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteEventCommand(id));
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event");
            return BadRequest("An error occurred while deleting the event");
        }
    }
}