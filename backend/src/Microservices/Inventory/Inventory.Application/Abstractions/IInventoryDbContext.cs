using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Inventory.Application.Abstractions;

public interface IInventoryDbContext
{
    DbSet<Item> Items { get; }
    DbSet<Movement> Movements { get; }
    DbSet<MovementType> MovementTypes { get; }
    DbSet<Location> Locations { get; }
    DbSet<LocationType> LocationTypes { get; }
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
