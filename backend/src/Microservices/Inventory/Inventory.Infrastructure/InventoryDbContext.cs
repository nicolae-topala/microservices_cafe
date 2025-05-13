using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options)
    : DbContext(options), IInventoryDbContext
{
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Movement> Movements => Set<Movement>();
    public DbSet<MovementType> MovementTypes => Set<MovementType>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<LocationType> LocationTypes => Set<LocationType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await base.SaveChangesAsync(cancellationToken);
}
