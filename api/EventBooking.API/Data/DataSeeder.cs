using EventBooking.API.Models;

namespace EventBooking.API.Data;

public static class DataSeeder
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        if (!context.Events.Any())
        {
            var createdBy = Guid.Parse("2dff81ac-7eb5-40d6-b661-1b8734295043");
            var events = new List<Event>
            {
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "TechCon 2025: AI & Future of Computing",
                    Description = "Join the premier tech conference of 2025 featuring keynotes from industry leaders at Google, Microsoft, and Amazon. Deep dive into AI, quantum computing, and sustainable tech. Includes hands-on workshops, networking sessions, and exclusive product demos.",
                    StartDate = DateTime.Parse("2025-09-15T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2026-09-17T17:00:00").ToUniversalTime(),
                    VenueName = "Silicon Valley Convention Center",
                    TotalSeats = 2000,
                    AvailableSeats = 750,
                    Price = 1299.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Coachella Music Festival 2025",
                    Description = "Experience the world's most iconic music festival featuring top artists across multiple genres. This year's lineup includes Taylor Swift, The Weeknd, and Coldplay. Enjoy art installations, gourmet food vendors, and unforgettable desert sunsets.",
                    StartDate = DateTime.Parse("2025-04-12T12:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2027-04-21T23:59:00").ToUniversalTime(),
                    VenueName = "Empire Polo Club, Indio",
                    TotalSeats = 125000,
                    AvailableSeats = 25000,
                    Price = 499.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Global Entrepreneurship Summit 2025",
                    Description = "Connect with successful entrepreneurs, venture capitalists, and industry innovators. Features startup pitch competitions, mentorship sessions, and workshops on scaling businesses globally. Special focus on sustainable and social entrepreneurship.",
                    StartDate = DateTime.Parse("2025-06-20T08:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2028-06-22T18:00:00").ToUniversalTime(),
                    VenueName = "World Trade Center",
                    TotalSeats = 1500,
                    AvailableSeats = 600,
                    Price = 899.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "FIFA World Cup Final 2025",
                    Description = "Witness history at the FIFA Women's World Cup Final 2025. Experience the culmination of the world's biggest football tournament with pre-match entertainment, fan zones, and exclusive memorabilia. Limited VIP packages available.",
                    StartDate = DateTime.Parse("2025-05-20T20:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-05-20T23:00:00").ToUniversalTime(),
                    VenueName = "Wembley Stadium",
                    TotalSeats = 90000,
                    AvailableSeats = 15000,
                    Price = 299.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "DevOps & Cloud Computing Conference",
                    Description = "Master modern DevOps practices and cloud technologies. Learn about containerization, microservices, CI/CD pipelines, and cloud-native architecture. Featuring speakers from AWS, Google Cloud, and Azure. Includes certification workshops.",
                    StartDate = DateTime.Parse("2025-05-15T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-05-17T17:00:00").ToUniversalTime(),
                    VenueName = "Tech Hub Conference Center",
                    TotalSeats = 800,
                    AvailableSeats = 350,
                    Price = 799.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Broadway Musical: Hamilton",
                    Description = "Experience the Tony Award-winning musical that has revolutionized theater. Follow Alexander Hamilton's journey through the founding of America with hip-hop, jazz, R&B, and Broadway musical numbers. Premium seating available.",
                    StartDate = DateTime.Parse("2025-08-10T19:30:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-08-10T22:30:00").ToUniversalTime(),
                    VenueName = "Richard Rodgers Theatre",
                    TotalSeats = 1319,
                    AvailableSeats = 200,
                    Price = 299.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Sustainable Fashion Week",
                    Description = "Discover the future of fashion with eco-friendly designers and sustainable brands. Features runway shows, panel discussions on ethical fashion, and sustainable textile exhibitions. Network with industry leaders and emerging designers.",
                    StartDate = DateTime.Parse("2025-09-05T10:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-09-11T20:00:00").ToUniversalTime(),
                    VenueName = "Fashion District Center",
                    TotalSeats = 3000,
                    AvailableSeats = 1200,
                    Price = 449.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Culinary Masters: Food & Wine Festival",
                    Description = "Indulge in a gastronomic adventure featuring Michelin-starred chefs, wine tastings, and cooking demonstrations. Experience global cuisines, artisanal products, and exclusive chef's table events. Includes wine pairing workshops.",
                    StartDate = DateTime.Parse("2025-10-01T11:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-10-03T22:00:00").ToUniversalTime(),
                    VenueName = "Grand Central Market",
                    TotalSeats = 5000,
                    AvailableSeats = 2000,
                    Price = 199.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Space Exploration Summit 2025",
                    Description = "Join astronauts, scientists, and space industry leaders for discussions on Mars missions, space tourism, and interstellar exploration. Features virtual reality space experiences and scale models of upcoming spacecraft.",
                    StartDate = DateTime.Parse("2025-11-12T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-11-14T17:00:00").ToUniversalTime(),
                    VenueName = "Space Center Houston",
                    TotalSeats = 1000,
                    AvailableSeats = 400,
                    Price = 599.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Digital Art & NFT Exhibition",
                    Description = "Explore the intersection of art and technology featuring leading digital artists and NFT creators. Interactive installations, VR art experiences, and blockchain art trading workshops. Special showcase of AI-generated artwork.",
                    StartDate = DateTime.Parse("2026-07-05T10:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2027-07-07T18:00:00").ToUniversalTime(),
                    VenueName = "Modern Art Gallery",
                    TotalSeats = 1200,
                    AvailableSeats = 500,
                    Price = 149.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Mental Health & Wellness Conference",
                    Description = "Join mental health professionals, wellness experts, and motivational speakers for a comprehensive look at mental well-being. Includes meditation sessions, stress management workshops, and professional networking.",
                    StartDate = DateTime.Parse("2025-05-18T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2028-05-19T17:00:00").ToUniversalTime(),
                    VenueName = "Wellness Center",
                    TotalSeats = 600,
                    AvailableSeats = 250,
                    Price = 249.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Electric Vehicle Expo 2025",
                    Description = "Experience the future of transportation with the latest electric vehicles, charging technology, and sustainable mobility solutions. Test drives available for new EV models. Special focus on urban mobility and infrastructure.",
                    StartDate = DateTime.Parse("2025-08-25T10:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-12-27T18:00:00").ToUniversalTime(),
                    VenueName = "Auto Convention Center",
                    TotalSeats = 5000,
                    AvailableSeats = 2200,
                    Price = 75.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Comic Con International 2025",
                    Description = "The ultimate pop culture convention featuring celebrity panels, comic artists, exclusive merchandise, and cosplay competitions. Meet your favorite actors from Marvel, DC, and sci-fi franchises. Gaming tournaments and preview screenings.",
                    StartDate = DateTime.Parse("2025-07-18T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-11-21T19:00:00").ToUniversalTime(),
                    VenueName = "Convention Center",
                    TotalSeats = 130000,
                    AvailableSeats = 45000,
                    Price = 199.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Blockchain & Cryptocurrency Summit",
                    Description = "Explore the latest in blockchain technology, DeFi, and cryptocurrency markets. Learn from industry experts about Web3, smart contracts, and regulatory compliance. Includes trading workshops and networking events.",
                    StartDate = DateTime.Parse("2025-06-08T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-10-09T17:00:00").ToUniversalTime(),
                    VenueName = "Financial District Center",
                    TotalSeats = 800,
                    AvailableSeats = 300,
                    Price = 699.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Photography World Expo",
                    Description = "The largest photography event featuring the latest camera gear, workshops by renowned photographers, and photo exhibitions. Includes landscape, portrait, and street photography sessions. Portfolio reviews available.",
                    StartDate = DateTime.Parse("2025-09-28T10:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-09-30T18:00:00").ToUniversalTime(),
                    VenueName = "Photography Museum",
                    TotalSeats = 2500,
                    AvailableSeats = 1000,
                    Price = 149.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Global Climate Action Forum",
                    Description = "Join environmental leaders, scientists, and activists for discussions on climate change solutions, renewable energy, and conservation. Features case studies of successful environmental initiatives and sustainable technology demos.",
                    StartDate = DateTime.Parse("2025-10-15T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-10-17T17:00:00").ToUniversalTime(),
                    VenueName = "Environmental Research Center",
                    TotalSeats = 1500,
                    AvailableSeats = 600,
                    Price = 299.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Jazz & Blues Festival 2025",
                    Description = "Three days of soulful music featuring legendary jazz and blues artists from around the world. Multiple stages, jam sessions, and masterclasses. Local food vendors and craft beer garden available.",
                    StartDate = DateTime.Parse("2025-08-30T16:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2025-09-01T23:00:00").ToUniversalTime(),
                    VenueName = "Riverfront Park",
                    TotalSeats = 15000,
                    AvailableSeats = 6000,
                    Price = 159.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "International Food Innovation Expo",
                    Description = "Discover the future of food with innovative food tech startups, sustainable farming solutions, and alternative protein products. Includes tastings, chef demonstrations, and discussions on food security and sustainability.",
                    StartDate = DateTime.Parse("2024-11-05T10:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2024-11-07T18:00:00").ToUniversalTime(),
                    VenueName = "Food Innovation Center",
                    TotalSeats = 3000,
                    AvailableSeats = 1200,
                    Price = 129.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                },
                new Event
                {
                    Id = Guid.NewGuid(),
                    Title = "Virtual Reality Gaming Championship",
                    Description = "The world's premier VR gaming competition featuring top e-sports teams. Experience the latest VR technology, game demos, and developer talks. Includes casual gaming zones and virtual reality art exhibitions.",
                    StartDate = DateTime.Parse("2023-12-01T09:00:00").ToUniversalTime(),
                    EndDate = DateTime.Parse("2023-12-03T20:00:00").ToUniversalTime(),
                    VenueName = "Gaming Arena",
                    TotalSeats = 5000,
                    AvailableSeats = 2000,
                    Price = 89.99M,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }
}