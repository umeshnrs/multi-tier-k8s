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
                    Title = "Web Development Summit 2024",
                    Description = "Join industry experts for an intensive two-day conference covering the latest in web development, including React, Vue, Node.js, and cloud architecture. Network with leading developers and participate in hands-on workshops.",
                    StartDate = DateTime.Parse("2024-06-15T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-06-16T17:00:00").ToUniversalTime(),
                    VenueName = "Tech Innovation Center",
                    TotalSeats = 500,
                    AvailableSeats = 127,
                    Price = 599.99M,
                    CreatedAt = DateTime.Parse("2024-01-15T08:00:00").ToUniversalTime(),
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Summer Jazz Festival",
                    Description = "Experience three magical evenings of world-class jazz performances under the stars. Featuring Grammy-winning artists, local talents, and fusion bands. Food vendors and wine tasting included.",
                    StartDate = DateTime.Parse("2024-07-20T17:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-07-22T23:00:00").ToUniversalTime(),
                    VenueName = "Riverside Amphitheater",
                    TotalSeats = 2000,
                    AvailableSeats = 856,
                    Price = 175.00M,
                    CreatedAt = DateTime.Parse("2024-01-20T10:30:00").ToUniversalTime(),
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "AI & Machine Learning Workshop",
                    Description = "Practical workshop on implementing AI solutions. Topics include deep learning, neural networks, and real-world applications. Includes hands-on sessions with industry-standard tools and take-home projects.",
                    StartDate = DateTime.Parse("2024-05-10T08:30:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-05-10T16:30:00").ToUniversalTime(),
                    VenueName = "Digital Learning Hub",
                    TotalSeats = 100,
                    AvailableSeats = 12,
                    Price = 349.99M,
                    CreatedAt = DateTime.Parse("2024-01-25T14:20:00").ToUniversalTime(),
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Startup Networking Breakfast",
                    Description = "Monthly breakfast meetup for startup founders, investors, and tech entrepreneurs. Features lightning talks, pitch practice, and structured networking sessions.",
                    StartDate = DateTime.Parse("2024-04-05T07:30:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-04-05T10:00:00").ToUniversalTime(),
                    VenueName = "Innovation Lounge",
                    TotalSeats = 75,
                    AvailableSeats = 25,
                    Price = 45.00M,
                    CreatedAt = DateTime.Parse("2024-01-28T09:15:00").ToUniversalTime(),
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Digital Marketing Masterclass",
                    Description = "Comprehensive one-day course covering SEO, social media marketing, content strategy, and analytics. Learn from successful campaign managers and digital marketing experts.",
                    StartDate = DateTime.Parse("2024-05-25T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-05-25T17:00:00").ToUniversalTime(),
                    VenueName = "Business Center Downtown",
                    TotalSeats = 150,
                    AvailableSeats = 83,
                    Price = 299.99M,
                    CreatedAt = DateTime.Parse("2024-02-01T11:00:00").ToUniversalTime(),
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Cybersecurity Conference 2024",
                    Description = "Annual cybersecurity conference featuring keynotes from leading security experts, workshops on threat detection, and the latest in security technologies. Includes certification preparation tracks.",
                    StartDate = DateTime.Parse("2024-08-15T08:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-08-17T18:00:00").ToUniversalTime(),
                    VenueName = "Metropolitan Convention Center",
                    TotalSeats = 800,
                    AvailableSeats = 342,
                    Price = 899.99M,
                    CreatedAt = DateTime.Parse("2024-02-05T13:45:00").ToUniversalTime(),
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Product Management Bootcamp",
                    Description = "Intensive three-day bootcamp for aspiring and junior product managers. Learn product strategy, user research, roadmap planning, and agile methodologies through real-world case studies.",
                    StartDate = DateTime.Parse("2024-06-03T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-06-05T17:00:00").ToUniversalTime(),
                    VenueName = "Product Innovation Campus",
                    TotalSeats = 120,
                    AvailableSeats = 47,
                    Price = 1299.99M,
                    CreatedAt = DateTime.Parse("2024-02-10T15:30:00").ToUniversalTime(),
                    CreatedBy = createdBy
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }
}