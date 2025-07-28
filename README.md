# API

# UI

# K8s

## Development Setup

### Prerequisites
- Docker Desktop
- Docker Compose

### First-time Setup

1. Create a development PostgreSQL volume:
```bash
docker volume create postgres_data_dev
```

2. Start the services:
```bash
docker-compose up -d
```

The database schema will be automatically created through Entity Framework Core migrations when the API starts up.

### Regular Development

- Start services: `docker-compose up -d`
- Stop services: `docker-compose down`
- View logs: `docker-compose logs -f`
- Rebuild services: `docker-compose up -d --build`

### Clean Start

If you need to start fresh:
1. Stop and remove everything: `docker-compose down -v`
2. Remove the development volume: `docker volume rm postgres_data_dev`
3. Create a new volume: `docker volume create postgres_data_dev`
4. Start services: `docker-compose up --build -d`

### Database Migrations

The application uses Entity Framework Core migrations to manage the database schema. When the API starts, it automatically applies any pending migrations.