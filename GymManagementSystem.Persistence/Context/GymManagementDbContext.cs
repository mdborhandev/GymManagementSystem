using GymManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Persistence.Context;

public class GymManagementDbContext : DbContext
{
    public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Gym> Gyms => Set<Gym>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global query filter for multi-tenancy on ITenantEntity types
        modelBuilder.Entity<Member>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Package>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Gym>().HasQueryFilter(g => !g.IsDeleted);
    }
}
