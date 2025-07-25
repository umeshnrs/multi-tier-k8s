# API Data Migration

# Navigate to the API project directory
cd api/EventBooking.API

# Install EF Core tools if not already installed
dotnet tool install --global dotnet-ef

# Create the initial migration
dotnet ef migrations add InitialCreate --output-dir Data/Migrations

# Apply the migration
dotnet ef database update

# UNIT Test
cd api/EventBooking.Test
dotnet test