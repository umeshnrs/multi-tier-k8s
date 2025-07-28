# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build

# Create development cert
RUN dotnet dev-certs https

WORKDIR /src

# Copy solution and project files first for better layer caching
COPY ["EventBooking.API.sln", "./"]
COPY ["EventBooking.API/EventBooking.API.csproj", "EventBooking.API/"]
COPY ["EventBooking.Test/EventBooking.Test.csproj", "EventBooking.Test/"]

# Restore dependencies
RUN dotnet restore

# Copy the rest of the files
COPY EventBooking.API/. EventBooking.API/
COPY EventBooking.Test/. EventBooking.Test/

# Build and test
RUN dotnet build -c Release /clp:ErrorsOnly
RUN dotnet test --no-build -c Release --verbosity normal

FROM build AS publish
ARG TIER=Release
RUN dotnet publish "EventBooking.API/EventBooking.API.csproj" -c $TIER \
    --property:PublishDir=/src/dist \
    --self-contained=true \
    --runtime linux-x64 \
    /p:WarningLevel=1 \
    /p:TreatWarningsAsErrors=false \
    /p:DisableImplicitNuGetFallbackFolder=true

FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-jammy AS prod

# Create a new user with home directory
RUN adduser --disabled-password --home /app --gecos '' dotnetuser

# Copy dev cert and configure default cert store location
RUN mkdir /app/cert
COPY --from=build ["/root/.dotnet/corefx/cryptography/x509stores/my/*", "/app/cert/cert.pfx"]
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/cert/cert.pfx"

# Change directory ownership
RUN chown -R dotnetuser /app

# Impersonate into the new user
USER dotnetuser
WORKDIR /app
COPY --from=publish /src/dist .

EXPOSE 5000 5001

ENTRYPOINT ["./EventBooking.API"]