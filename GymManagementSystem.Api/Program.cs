using GymManagementSystem.Api.Services;
using GymManagementSystem.Application;
using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Persistence;
using GymManagementSystem.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();

// Configure Persistence Layer (Must be called before Identity)
builder.Services.AddPersistence(builder.Configuration);

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<GymManagementDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Configure Layers
builder.Services.AddApplication();

var app = builder.Build();

// ... existing database migration logic ...
// (Apply pending migrations and seed data)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GymManagementDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    try
    {
        await dbContext.Database.MigrateAsync();
        
        // SEED ROLES
        if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("SuperAdmin"));
        
        if (!await roleManager.RoleExistsAsync("GymAdmin"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("GymAdmin"));

        // SEED SUPERADMIN USER (Platform Manager)
        var superAdminEmail = "superadmin@gymsaas.com";
        var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);
        if (superAdminUser == null)
        {
            superAdminUser = new ApplicationUser
            {
                UserName = superAdminEmail,
                Email = superAdminEmail,
                FirstName = "Platform",
                LastName = "SuperAdmin",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(superAdminUser, "SuperAdmin123");
            await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogCritical(ex, "Database startup or seeding failed.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Gym Management System API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
