namespace EventBooking.API.Features.Events.Dtos;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; init; } = new List<T>();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}