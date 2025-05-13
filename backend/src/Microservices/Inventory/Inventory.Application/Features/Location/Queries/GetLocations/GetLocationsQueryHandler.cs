using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging;
using LocationDomain = Inventory.Domain.Entities.Location;

namespace Inventory.Application.Features.Location.Queries.GetLocations;

public class GetLocationsQueryHandler(IInventoryDbContext dbContext) 
    : IQueryHandler<GetLocationsQuery, IQueryable<LocationDomain>>
{
    public Task<IQueryable<LocationDomain>> Handle(GetLocationsQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(dbContext.Locations
            .Include(l => l.LocationType)
            .AsNoTracking());
}
