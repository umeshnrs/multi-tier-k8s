# Event Booking API

A .NET 9 API for managing event bookings with clean architecture, CQRS pattern, and comprehensive test coverage.

## Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Database Migrations](#database-migrations)
- [Contributing](#contributing)
- [License](#license)

## Features

- Event creation and management
- Clean Architecture implementation
- CQRS pattern with MediatR
- FluentValidation for request validation
- PostgreSQL database integration
- Comprehensive unit tests
- Swagger API documentation

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/downloads)

## Getting Started

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/umeshnrs/multi-tier-k8s.git
   cd multi-tier-k8s
   ```

2. Install required .NET tools:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

### Configuration

1. Navigate to the API project:
   ```bash
   cd EventBooking.API
   ```

2. Update database connection in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=EventBookingDb;Username=your_username;Password=your_password"
     }
   }
   ```

### Database Setup

1. Ensure PostgreSQL is running locally

2. Apply database migrations:
   ```bash
   cd EventBooking.API
   dotnet ef database update
   ```
   This will:
   - Create the database if it doesn't exist
   - Apply all migrations
   - Seed initial event data

## Running the Application

1. Navigate to the API project:
   ```bash
   cd EventBooking.API
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Access the API:
   - Swagger UI: https://localhost:5000/swagger/index.html
   - API Base URL: https://localhost:5000/api

## Testing

1. Navigate to the test project:
   ```bash
   cd EventBooking.Test
   ```

2. Run the tests:
   ```bash
   dotnet test
   ```

## Project Structure

```
./
├── EventBooking.API/
│   ├── Controllers/         # API endpoints
│   ├── Data/               # Database context and migrations
│   ├── Features/           # CQRS features (commands/queries)
│   │   └── Events/         # Event-related features
│   │       ├── Commands/   # Create, Update, Delete operations
│   │       ├── Queries/    # Read operations
│   │       ├── Dtos/      # Data transfer objects
│   │       └── Mappings/   # AutoMapper profiles
│   ├── Models/             # Domain models
│   └── Interfaces/         # Abstractions
└── EventBooking.Test/
    ├── Controllers/        # Controller tests
    └── Features/           # Feature tests
        └── Events/         # Event feature tests
            ├── Commands/   # Command tests
            └── Queries/    # Query tests
```

## API Documentation

- API documentation is available via Swagger UI at: https://localhost:5000/swagger/index.html
- Detailed endpoint documentation including:
  - Request/Response models
  - Authentication requirements
  - Example requests

## Database Migrations

### Creating New Migrations

1. Navigate to the API project:
   ```bash
   cd EventBooking.API
   ```

2. Create a new migration:
   ```bash
   dotnet ef migrations add [MigrationName] --output-dir Data/Migrations
   ```

3. Apply the migration:
   ```bash
   dotnet ef database update
   ```

### Rolling Back Migrations

To rollback to a specific migration:
```bash
cd EventBooking.API
dotnet ef database update [MigrationName]
```

### Remove Last Migration

If you need to remove the last migration (if it hasn't been applied):
```bash
cd EventBooking.API
dotnet ef migrations remove
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---
For any additional questions or issues, please open a GitHub issue.