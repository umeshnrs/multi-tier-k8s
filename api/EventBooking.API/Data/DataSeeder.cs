using EventBooking.API.Models;

namespace EventBooking.API.Data;

public static class DataSeeder
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        if (!context.Events.Any())
        {
            var createdBy = Guid.NewGuid();// This should be replaced with the actual user ID who is creating the events
            var events = new List<Event>
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Tech Conference 2025",
                    Description = "Annual technology conference featuring the latest in AI and cloud computing",
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(600),
                    VenueName = "Tech Convention Center",
                    TotalSeats = 1000,
                    AvailableSeats = 1000,
                    Price = 299.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Music Festival 2025",
                    Description = "Three days of non-stop music featuring top artists from around the world",
                    StartDate = DateTime.UtcNow.AddDays(-45),
                    EndDate = DateTime.UtcNow.AddDays(470),
                    VenueName = "Central Park",
                    TotalSeats = 5000,
                    AvailableSeats = 5000,
                    Price = 199.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Food & Wine Expo 2025",
                    Description = "Explore culinary delights and wine tasting from renowned chefs",
                    StartDate = DateTime.UtcNow.AddDays(15),
                    EndDate = DateTime.UtcNow.AddDays(16),
                    VenueName = "Grand Hotel",
                    TotalSeats = 500,
                    AvailableSeats = 500,
                    Price = 149.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Business Leadership Summit 2025",
                    Description = "Connect with industry leaders and learn about future business trends",
                    StartDate = DateTime.UtcNow.AddDays(60),
                    EndDate = DateTime.UtcNow.AddDays(619),
                    VenueName = "Business Center",
                    TotalSeats = 300,
                    AvailableSeats = 300,
                    Price = 499.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Art Exhibition 2025",
                    Description = "Explore the latest in contemporary art and design",
                    StartDate = DateTime.UtcNow.AddDays(75),
                    EndDate = DateTime.UtcNow.AddDays(760),
                    VenueName = "Art Gallery",
                    TotalSeats = 200,
                    AvailableSeats = 200,
                    Price = 19.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Fashion Week 2025",
                    Description = "Explore the latest in fashion and design",
                    StartDate = DateTime.UtcNow.AddDays(90),
                    EndDate = DateTime.UtcNow.AddDays(910),
                    VenueName = "Fashion Center",
                    TotalSeats = 1000,
                    AvailableSeats = 1000,
                    Price = 499.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Sales Conference 2025",
                    Description = "Annual technology conference featuring the latest in AI and cloud computing",
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddDays(32),
                    VenueName = "Tech Convention Center",
                    TotalSeats = 1000,
                    AvailableSeats = 1000,
                    Price = 599.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Marketing Conference 2025",
                    Description = "Annual technology conference featuring the latest in AI and cloud computing",
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddDays(32),
                    VenueName = "Tech Convention Center",
                    TotalSeats = 1000,
                    AvailableSeats = 1000,
                    Price = 99.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }
}