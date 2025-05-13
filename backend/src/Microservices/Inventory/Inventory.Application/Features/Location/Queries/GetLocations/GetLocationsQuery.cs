using Shared.Abstractions.Messaging;
using LocationDomain = Inventory.Domain.Entities.Location;

namespace Inventory.Application.Features.Location.Queries.GetLocations;

public record GetLocationsQuery : IQuery<IQueryable<LocationDomain>>;
