using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using LocationDomain = Inventory.Domain.Entities.Location;

namespace Inventory.Application.Features.Location.Commands.CreateLocation;

public class CreateLocationCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<CreateLocationCommand, LocationDomain>
{
    public async Task<Result<LocationDomain>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var locationType = await dbContext.LocationTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Location.LocationTypeId, cancellationToken);

        if (locationType is null)
        {
            return Result.Failure<LocationDomain>(new ResultError("Location.InvalidLocationType", "Location type not found"));
        }

        var locationResult = LocationDomain.Create(
        request.Location.Name,
        request.Location.Address,
        locationType);

        if (locationResult.IsFailure)
        {
            return Result.Failure<LocationDomain>(locationResult.Error);
        }

        await dbContext.Locations
            .AddAsync(locationResult.Value, cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure<LocationDomain>(new ResultError("Location.SaveFailed",
                $"Failed to save location: {ex.InnerException?.Message ?? ex.Message}"));
        }

        return Result.Success(locationResult.Value);
    }
}
