## SNOW BUDDIES BACKEND REPOSITORY with ASP.NET Core

[![CI](https://github.com/Snow-Buddies-App/snow-buddies/actions/workflows/ci.yaml/badge.svg)](https://github.com/Snow-Buddies-App/snow-buddies/actions/workflows/ci.yaml)

Snow Buddies is a mobile application designed to enhance the skiing and snowboarding experience by facilitating communication and meetups among friends on the mountain. Whether you're hitting the slopes with your regular crew or embarking on a trip with new companions, Snow Buddies helps you stay connected, coordinate meetups, and navigate large resorts more effectively.

### Tech stack

- ASP.NET Core on .NET 10
- PostgreSQL with Entity Framework Core (migrations run automatically at startup)
- JWT bearer authentication, plus Google Sign-In
- Serilog logging, Swagger UI at `/swagger`, health check at `/health`

### Project layout

| Project | Purpose |
|---|---|
| `src/SnowBuddies.Api` | Controllers, request models, middleware, startup |
| `src/SnowBuddies.Application` | Services, DTOs, interfaces, settings |
| `src/SnowBuddies.Domain` | Entities and domain exceptions |
| `src/SnowBuddies.Infrastructure` | EF Core `DbContext`, repositories, migrations |
| `tests/UnitTests` | xUnit tests |

### Running locally

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download) and Docker (for Postgres).

```bash
# 1. Start Postgres
docker run -d --name snowbuddies-db -p 5432:5432 -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=SnowBuddiesDb postgres:17

# 2. Configure secrets (stored outside the repo)
cd src/SnowBuddies.Api
dotnet user-secrets set "Password" "postgres"
dotnet user-secrets set "AppSettings:Token" "$(openssl rand -base64 48)"
dotnet user-secrets set "Authentication:Google:ClientIds" "<your-google-client-id>"   # optional

# 3. Run (applies migrations, then serves Swagger at http://localhost:5000/swagger)
dotnet run
```

Run the tests with `dotnet test SnowBuddies.sln`.

### Configuration

Every setting can be supplied as an environment variable (use `__` in place of `:`).

| Setting | Required | Description |
|---|---|---|
| `DATABASE_URL` or `ConnectionStrings__SBConnectionString` | Yes | Postgres connection, as a `postgres://user:pass@host:port/db` URL or an Npgsql connection string |
| `Password` | No | Overrides the password in the connection string |
| `AppSettings__Token` | Yes | JWT signing secret, at least 32 characters |
| `Authentication__Google__ClientIds` | For Google Sign-In | Comma-separated Google OAuth client IDs (iOS, Android, web) |
| `Jwt__Issuer`, `Jwt__Audience`, `Jwt__ExpiryMinutes` | No | Token settings (defaults: `snow-buddies-api`, `snow-buddies-app`, `60`) |
| `Database__MigrateOnStartup` | No | Apply EF migrations at startup (default `true`) |
| `Swagger__Enabled` | No | Serve Swagger UI outside Development (default `true`) |
| `RateLimiting__AuthPermitsPerMinute` | No | Per-IP limit on login and register (default `10`) |
| `PORT` | No | Port to listen on; set automatically by most hosts |

### API overview

All endpoints except register, login and health require `Authorization: Bearer <token>`.

| Method | Route | Notes |
|---|---|---|
| `POST` | `/api/User/Register` | Create an account with email and password |
| `POST` | `/api/Auth/Login` | Returns `{ token, expiresAtUtc, userId }` |
| `POST` | `/api/Auth/Google-sign-in` | Body `{ "idToken": "<Google ID token>" }`; creates the account on first sign-in; same response as login |
| `GET` | `/api/User`, `/api/User/me`, `/api/User/{id}` | Read users |
| `PUT` / `DELETE` | `/api/User/{id}` | Only on your own account |
| `GET` | `/api/UserProfile`, `/api/UserProfile/me`, `/api/UserProfile/{id}` | Read profiles |
| `PUT` | `/api/UserProfile/edit-profile/{id}` | Only on your own profile |
| `GET` | `/health` | Returns `Healthy` when the API can reach the database |

Errors are returned as `{ "statusCode": 409, "message": "..." }`.

### Deployment

The API ships as a Docker image (see `Dockerfile`) and includes a one-click [Render](https://render.com) blueprint (`render.yaml`). See [docs/DEPLOYMENT.md](docs/DEPLOYMENT.md).
