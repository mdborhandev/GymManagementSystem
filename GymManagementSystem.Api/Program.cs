using GymManagementSystem.Api.Services;
using GymManagementSystem.Application;
using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Persistence;
using GymManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();

// Configure Layers
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

// Apply pending migrations and validate DB connectivity during startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GymManagementDbContext>();

    try
    {
        await dbContext.Database.MigrateAsync();

        if (!await dbContext.Database.CanConnectAsync())
        {
            throw new InvalidOperationException("Database connectivity validation failed after migration.");
        }

        app.Logger.LogInformation("Database migration and connectivity check completed successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogCritical(ex, "Database startup validation failed. Check PostgreSQL and connection string.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Add this to use Swagger UI with the generated OpenAPI spec
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Gym Management System API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
