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
    "DefaultConnection": "Host=localhost;Port=5432;Database=eventbooking;Username=postgres;Password=local"
  },
  "AllowedOrigins": [
    "http://localhost:5173",
    "http://localhost"
  ]
}
```

#### Using Environment Variables:
```bash
# Database Configuration
DB_HOST=localhost
DB_PORT=5432
DB_NAME=eventbooking
DB_USER=postgres
DB_PASSWORD=local

# CORS Configuration
ALLOWED_ORIGINS=http://localhost:5173,http://localhost

# ASP.NET Core Configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://+:5000;http://+:5001
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

3. Access the API:
   - Swagger UI: https://localhost:5000/swagger/index.html
   - API Base URL: https://localhost:5000/api

### Running with Docker

1. Build the Docker image:
   ```bash
   # From the api directory
   docker build -t eventbooking-api -f EventBooking.API/Dockerfile .
   ```

2. Run with Docker:

   For Windows (Docker Desktop):
   ```bash
   docker run -d \
     --name eventbooking-api \
     --add-host=host.docker.internal:host-gateway \
     -p 5000:5000 \
     -p 5001:5001 \
     -e ASPNETCORE_ENVIRONMENT=Development \
     -e DB_HOST=host.docker.internal \
     -e DB_PORT=5432 \
     -e DB_NAME=eventbooking \
     -e DB_USER=postgres \
     -e DB_PASSWORD=local \
     eventbooking-api
   ```

   For Linux:
   ```bash
   # Option 1: Using host network (recommended)
   docker run -d \
     --name eventbooking-api \
     --network=host \
     -e ASPNETCORE_ENVIRONMENT=Development \
     -e DB_HOST=localhost \
     -e DB_PORT=5432 \
     -e DB_NAME=eventbooking \
     -e DB_USER=postgres \
     -e DB_PASSWORD=local \
     eventbooking-api

   # Option 2: Using host IP address
   docker run -d \
     --name eventbooking-api \
     -p 5000:5000 \
     -p 5001:5001 \
     -e ASPNETCORE_ENVIRONMENT=Development \
     -e DB_HOST=172.17.0.1 \
     -e DB_PORT=5432 \
     -e DB_NAME=eventbooking \
     -e DB_USER=postgres \
     -e DB_PASSWORD=local \
     eventbooking-api
   ```

   Notes: 
   - For Windows (Docker Desktop): 
     - Use `host.docker.internal` with `--add-host=host.docker.internal:host-gateway` flag
     - This allows the container to resolve the host machine's address
   - For Linux: 
     - Either use `--network=host` (recommended) or the host's IP address
     - Default docker0 bridge is usually 172.17.0.1
   - Make sure PostgreSQL is configured to accept connections from Docker:
     1. Update postgresql.conf:
        ```
        listen_addresses = '*'
        ```
     2. Update pg_hba.conf to allow Docker subnet:
        ```
        host    all             all             172.17.0.0/16           md5
        host    all             all             host.docker.internal    md5
        ```

3. Verify the connection:
   ```bash
   # Check container logs
   docker logs eventbooking-api
   ```

   If you see migration and seeding messages without errors, the connection is successful.

4. Access the API:
   - Swagger UI: https://localhost:5000/swagger/index.html
   - API Base URL: https://localhost:5000/api
   - HTTP API Base URL: http://localhost:5001/api

### Environment Variables

The application supports the following environment variables:

#### Database Configuration
- `DB_HOST` - PostgreSQL host (default: localhost)
- `DB_PORT` - PostgreSQL port (default: 5432)
- `DB_NAME` - Database name (default: eventbooking)
- `DB_USER` - Database username (default: postgres)
- `DB_PASSWORD` - Database password

#### CORS Configuration
- `ALLOWED_ORIGINS` - Comma-separated list of allowed origins for CORS

#### ASP.NET Core Configuration
- `ASPNETCORE_ENVIRONMENT` - Runtime environment (Development/Staging/Production)
- `ASPNETCORE_URLS` - URLs to listen on (default: https://+:5000;http://+:5001)
- `ASPNETCORE_Kestrel__Certificates__Default__Path` - Path to SSL certificate
- `ASPNETCORE_Kestrel__Certificates__Default__Password` - SSL certificate password

### Docker Features

The Docker setup includes:

1. Multi-stage build for optimized image size
2. Automatic test execution during build
3. Built-in HTTPS certificate handling
4. Non-root user for security
5. Environment variable support
6. PostgreSQL connection via localhost

### HTTPS Certificate Handling

The Dockerfile automatically handles HTTPS certificate creation and configuration:

1. Development certificate is created during build
2. Certificate is stored in `/app/cert/cert.pfx`
3. Certificate path is configured via environment variable
4. Non-root user has appropriate permissions

For production deployments:

1. Mount your production certificate:
   ```bash
   docker run -d \
     --name eventbooking-api \
     -p 5000:5000 \
     -p 5001:5001 \
     -v /path/to/your/cert:/app/cert/prod-cert.pfx \
     -e ASPNETCORE_Kestrel__Certificates__Default__Path=/app/cert/prod-cert.pfx \
     -e ASPNETCORE_Kestrel__Certificates__Default__Password=your_cert_password \
     --env-file .env \
     eventbooking-api
   ```

2. Or use a certificate from a secret store:
   ```bash
   docker run -d \
     --name eventbooking-api \
     -p 5000:5000 \
     -p 5001:5001 \
     --secret ssl-cert \
     -e ASPNETCORE_Kestrel__Certificates__Default__Path=/run/secrets/ssl-cert \
     -e ASPNETCORE_Kestrel__Certificates__Default__Password=your_cert_password \
     --env-file .env \
     eventbooking-api
   ```

Note: For production, always use proper SSL certificates from a trusted certificate authority.

### Development HTTPS Certificate Setup

For development, you'll need to create and trust a development certificate:

1. Create a development certificate:
   ```bash
   dotnet dev-certs https -ep ${HOME}/.aspnet/https/dev-cert.pfx -p your_cert_password
   dotnet dev-certs https --trust
   ```

2. Mount the certificate when running Docker:
   ```bash
   docker run -d \
     --name eventbooking-api \
     -p 5000:5000 \
     -p 80:80 \
     -v ${HOME}/.aspnet/https:/app/cert \
     --env-file .env \
     eventbooking-api
   ```

Note: For production, you should use a proper SSL certificate from a trusted certificate authority.

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