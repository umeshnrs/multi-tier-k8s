# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["EventBooking.API/*.csproj", "EventBooking.API/"]
RUN dotnet restore "EventBooking.API/EventBooking.API.csproj"

# Copy the rest of the files and build
COPY . .
WORKDIR "/src/EventBooking.API"
RUN dotnet build "EventBooking.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "EventBooking.API.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "EventBooking.API.dll"]