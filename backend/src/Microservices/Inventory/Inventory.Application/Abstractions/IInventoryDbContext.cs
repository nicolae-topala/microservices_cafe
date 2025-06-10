using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;

namespace Inventory.Application.Abstractions;

public interface IInventoryDbContext : IHasOutboxMessages, IDbContext
{
    DbSet<Item> Items { get; }
    DbSet<Movement> Movements { get; }
    DbSet<MovementType> MovementTypes { get; }
    DbSet<Location> Locations { get; }
    DbSet<LocationType> LocationTypes { get; }
}
