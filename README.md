# Gym Management System

## Prerequisites
- .NET SDK `10.0.x`
- PostgreSQL running locally on `localhost:5432`

## Quick Run
1. From the repository root, restore and build:
   ```bash
   dotnet restore GymManagementSystem.slnx
   dotnet build GymManagementSystem.slnx -c Debug --no-restore
   ```
2. Start the API:
   ```bash
   dotnet run --project GymManagementSystem.Api
   ```
3. Open Swagger/OpenAPI in development:
   - `http://localhost:5233/openapi/v1.json` (or configured launch URL/port)

## Database
- Default connection string is in [GymManagementSystem.Api/appsettings.json](/home/borhan-uddin-fahim/DRIVE A/Projects/GymManagementSystem/GymManagementSystem.Api/appsettings.json):
  - `Host=localhost;Port=5432;Database=gymmanagementdb;Username=postgres;Password=fahim123`
- On startup, the API now:
  - applies pending EF Core migrations
  - validates DB connectivity
  - fails fast with a clear log if DB is not reachable

## Common Issues
- `MSB4276` about missing SDKs like `Microsoft.NET.SDK.WorkloadAutoImportPropsLocator`:
  - your .NET SDK installation is incomplete/corrupted
  - fix by reinstalling/repairing .NET 10 SDK, then rerun restore/build

- Startup fails with DB connection errors:
  - ensure PostgreSQL is running
  - verify database credentials and host/port in `appsettings.json`

## Optional: Manual Migration Command
Run this from the repository root if you want manual control:
```bash
dotnet ef database update --project GymManagementSystem.Persistence --startup-project GymManagementSystem.Api
```
