using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;
using GymManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Persistence.Context;

public class GymManagementDbContext : DbContext
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

        // Apply global query filter for soft delete and multi-tenancy
        modelBuilder.Entity<Member>().HasQueryFilter(m => !m.IsDelete && m.GymId == _currentTenantService.TenantId);
        modelBuilder.Entity<Package>().HasQueryFilter(p => !p.IsDelete && p.GymId == _currentTenantService.TenantId);
        modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDelete && p.GymId == _currentTenantService.TenantId);
        modelBuilder.Entity<Staff>().HasQueryFilter(s => !s.IsDelete && s.GymId == _currentTenantService.TenantId);
        modelBuilder.Entity<Attendance>().HasQueryFilter(a => !a.IsDelete && a.GymId == _currentTenantService.TenantId);
        modelBuilder.Entity<Subscription>().HasQueryFilter(s => !s.IsDelete && s.GymId == _currentTenantService.TenantId);
        
        // Gym itself might not need GymId filter or it is the root
        modelBuilder.Entity<Gym>().HasQueryFilter(g => !g.IsDelete);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (_currentTenantService.TenantId.HasValue)
                {
                    entry.Entity.GymId = _currentTenantService.TenantId.Value;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
