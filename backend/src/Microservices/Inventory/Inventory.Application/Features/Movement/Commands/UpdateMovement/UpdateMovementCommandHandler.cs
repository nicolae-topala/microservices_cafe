using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using LocationDomain = Inventory.Domain.Entities.Location;

namespace Inventory.Application.Features.Movement.Commands.UpdateMovement;

public class UpdateMovementCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<UpdateMovementCommand>
{
    public async Task<Result> Handle(UpdateMovementCommand request, CancellationToken cancellationToken)
    {
        var movement = await dbContext.Movements
            .FirstOrDefaultAsync(x => x.Id == request.Movement.MovementId, cancellationToken);
        if (movement is null)
        {
            return Result.Failure(new ResultError("Movement.NotFound", "Movement not found"));
        }

        LocationDomain? location = null;
        if (request.Movement.LocationId.HasValue)
        {
            location = await dbContext.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Movement.LocationId, cancellationToken);

            if (location is null)
            {
                return Result.Failure(new ResultError("Movement.InvalidLocation", "Location not found"));
            }
        }

        MovementType? movementType = null;
        if (request.Movement.MovementTypeId.HasValue)
        {
            movementType = await dbContext.MovementTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Movement.MovementTypeId, cancellationToken);
            if (movementType is null)
            {
                return Result.Failure(new ResultError("Movement.InvalidMovementType", "Movement type not found"));
            }
        }

        var movementResult = movement.Update(request.Movement.Quantity, movementType, location, request.Movement.MovementDate);

        if (movementResult.IsFailure)
        {
            return Result.Failure(movementResult.Error);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
