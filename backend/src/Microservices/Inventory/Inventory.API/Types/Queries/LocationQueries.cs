using Inventory.Application.Features.Location.Queries.GetLocations;
using MediatR;
using Location = Inventory.Domain.Entities.Location;

namespace Inventory.API.Types.Queries;

[QueryType]
public class LocationQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Location>> GetLocations(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new GetLocationsQuery(), cancellationToken);
}
