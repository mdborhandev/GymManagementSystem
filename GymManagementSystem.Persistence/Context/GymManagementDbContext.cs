using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Interfaces;
using GymManagementSystem.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Persistence.Context;

public class GymManagementDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    private readonly ICurrentTenantService _currentTenantService;

    public GymManagementDbContext(
        DbContextOptions<GymManagementDbContext> options,
        ICurrentTenantService currentTenantService)
        : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Gym> Gyms => Set<Gym>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        // STRICT SAAS ISOLATION:
        modelBuilder.Entity<Member>().HasQueryFilter(m => !m.IsDelete && (_currentTenantService.TenantId == null || m.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Package>().HasQueryFilter(p => !p.IsDelete && (_currentTenantService.TenantId == null || p.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDelete && (_currentTenantService.TenantId == null || p.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Staff>().HasQueryFilter(s => !s.IsDelete && (_currentTenantService.TenantId == null || s.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Attendance>().HasQueryFilter(a => !a.IsDelete && (_currentTenantService.TenantId == null || a.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Subscription>().HasQueryFilter(s => !s.IsDelete && (_currentTenantService.TenantId == null || s.CompanyId == _currentTenantService.TenantId));
        
        modelBuilder.Entity<Gym>().HasQueryFilter(g => !g.IsDelete);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userEmail = _currentTenantService.CurrentUserName ?? "System";

        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = userEmail;
                
                // SaaS: Automatically bind the company ID
                if (_currentTenantService.TenantId.HasValue)
                {
                    entry.Entity.CompanyId = _currentTenantService.TenantId.Value;
                }
                else if (entry.Entity is Gym gym)
                {
                    // For the Company itself, its CompanyId is its own Id
                    if (gym.Id == Guid.Empty) gym.Id = Guid.NewGuid();
                    gym.CompanyId = gym.Id;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = userEmail;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
