# Slotify - Backend API

The backend for the Slotify Appointment Management System, built with .NET 6 following Clean Architecture principles.

## Architecture
The solution is divided into four main projects:
- **Slotify.Domain**: Contains core entities (User, Client, Service, Appointment) and enums.
- **Slotify.Application**: Contains business logic, DTOs, Mapping profiles (AutoMapper), and Validators (FluentValidation).
- **Slotify.Infrastructure**: Handles data persistence (EF Core), Identity, and external services.
- **Slotify.Api**: The entry point, containing Controllers, Middlewares, and Program.cs configuration.

## Tech Stack
- **Framework**: ASP.NET Core 6.0
- **ORM**: Entity Framework Core
- **Database**: SQL Server (LocalDB)
- **Auth**: JWT (JSON Web Tokens)
- **Logging**: Serilog

## Getting Started

### Prerequisites
- .NET 6 SDK
- SQL Server LocalDB

### Configuration
Update the connection string in `AppointmentManager.Api/appsettings.json` if necessary:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SlotifyDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Installation
1. Restore tools and packages:
   ```bash
   dotnet tool restore
   dotnet restore
   ```
2. Apply migrations:
   ```bash
   dotnet ef database update --project AppointmentManager.Infrastructure --startup-project AppointmentManager.Api
   ```
3. Run the development server:
   ```bash
   dotnet run --project AppointmentManager.Api
   ```

## Key API Endpoints
- `POST /api/auth/login` | `register`
- `GET/POST/PUT/DELETE /api/clients`
- `GET/POST/PUT /api/services`
- `GET/POST/DELETE /api/appointments`
- `GET /api/availability?date=YYYY-MM-DD`
