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
  - [Running Locally](#running-locally)
  - [Running with Docker](#running-with-docker)
  - [Environment Variables](#environment-variables)
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
- Docker support with HTTPS
- Environment-based configuration

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started) (optional)
- [Git](https://git-scm.com/downloads)

## Getting Started

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/umeshnrs/multi-tier-k8s.git
   cd multi-tier-k8s/api
   ```

2. Install required .NET tools:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

### Configuration

The application can be configured either through appsettings.json or environment variables.

#### Using appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=<DB_HOST>;Port=<DB_PORT>;Database=<DB_NAME>;Username=<DB_USER>;Password=<DB_PASSWORD>"
  },
  "AllowedOrigins": [
    "http://localhost:5173",
    "http://localhost"
  ]
}
```

#### Using Environment Variables:
```bash
# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://*:5000;https://*:5001

# Database Configuration
ConnectionStrings__DefaultConnection=Host=<your-db-host>;Port=<your-db-port>;Database=<your-db-name>;Username=<your-db-user>;Password=<your-db-password>

# CORS Configuration
AllowedOrigins__0=http://localhost:5173,http://localhost:80
```

### Database Setup

1. Ensure PostgreSQL is running locally

2. Apply database migrations:
   ```bash
   cd EventBooking.API
   dotnet ef database update
   ```

## Running the Application

### Running Locally

1. Navigate to the API project:
   ```bash
   cd EventBooking.API
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

### API Endpoints

When running, the API is accessible at:
- Swagger UI: https://localhost:5000/swagger/index.html
- API Base URL: https://localhost:5000/api
- HTTP API Base URL: http://localhost:5001/api

### Configuration

The application can be configured using environment variables or appsettings.json. See the root README.md for deployment instructions.



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