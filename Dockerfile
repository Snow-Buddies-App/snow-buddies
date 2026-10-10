# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first so the package layer is cached between code changes.
COPY SnowBuddies.sln ./
COPY src/SnowBuddies.Domain/SnowBuddies.Domain.csproj src/SnowBuddies.Domain/
COPY src/SnowBuddies.Application/SnowBuddies.Application.csproj src/SnowBuddies.Application/
COPY src/SnowBuddies.Infrastructure/SnowBuddies.Infrastructure.csproj src/SnowBuddies.Infrastructure/
COPY src/SnowBuddies.Api/SnowBuddies.Api.csproj src/SnowBuddies.Api/
RUN dotnet restore src/SnowBuddies.Api/SnowBuddies.Api.csproj

COPY src/ src/
RUN dotnet publish src/SnowBuddies.Api/SnowBuddies.Api.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Run as the non-root user that ships with the .NET images.
USER $APP_UID
ENTRYPOINT ["dotnet", "SnowBuddies.Api.dll"]
