namespace EventBooking.API.Features.Events.Dtos;

public record EventDto
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
    public DateTime CreatedAt { get; init; }
}