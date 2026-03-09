using GymManagementSystem.Application.Interfaces.Services;
using GymManagementSystem.Domain.Entities;
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

    #region DbSets
    public DbSet<Gym> Gyms => Set<Gym>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        // Force mapping of BaseModel properties for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseModel.CompanyId)).IsRequired();
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseModel.CreatedAt)).IsRequired();
                modelBuilder.Entity(entityType.ClrType).Property(nameof(BaseModel.IsDelete)).IsRequired();
            }
        }

        // SaaS Data Isolation: Global Filters
        // We filter by BaseModel.CompanyId automatically
        modelBuilder.Entity<Member>().HasQueryFilter(m => !m.IsDelete && (_currentTenantService.TenantId == null || m.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Package>().HasQueryFilter(p => !p.IsDelete && (_currentTenantService.TenantId == null || p.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDelete && (_currentTenantService.TenantId == null || p.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Staff>().HasQueryFilter(s => !s.IsDelete && (_currentTenantService.TenantId == null || s.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Attendance>().HasQueryFilter(a => !a.IsDelete && (_currentTenantService.TenantId == null || a.CompanyId == _currentTenantService.TenantId));
        modelBuilder.Entity<Subscription>().HasQueryFilter(s => !s.IsDelete && (_currentTenantService.TenantId == null || s.CompanyId == _currentTenantService.TenantId));
        
        // SuperAdmins manage all Gyms (Companies)
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
                
                // SaaS: Automatically bind the company ID from the session
                if (_currentTenantService.TenantId.HasValue)
                {
                    entry.Entity.CompanyId = _currentTenantService.TenantId.Value;
                }
                else if (entry.Entity is Gym gym)
                {
                    // Special case for Gym (The Company itself)
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
