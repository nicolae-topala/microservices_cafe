using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Inventory.Application.Features.Location.Commands.UpdateLocation;

public class UpdateLocationCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<UpdateLocationCommand>
{
    public async Task<Result> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        LocationType? locationType = null;
        if (request.Location.LocationTypeId.HasValue)
        {
            locationType = await dbContext.LocationTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Location.LocationTypeId, cancellationToken);

            if (locationType is null)
            {
                return Result.Failure(new ResultError("Location.InvalidLocationType", "Location type not found"));
            }
        }

        var location = await dbContext.Locations
            .FirstOrDefaultAsync(x => x.Id == request.Location.LocationId, cancellationToken);

        if (location is null)
        {
            return Result.Failure(new ResultError("Location.NotFound", "Location not found"));
        }

        var locationResult = location.Update(request.Location.Name, request.Location.Address, locationType);

        if (locationResult.IsFailure)
        {
            return Result.Failure(locationResult.Error);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
