using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using MovementDomain = Inventory.Domain.Entities.Movement;

namespace Inventory.Application.Features.Movement.Commands.CreateMovement;

public class CreateMovementCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<CreateMovementCommand, MovementDomain>
{
    public async Task<Result<MovementDomain>> Handle(CreateMovementCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Movement.ItemId, cancellationToken);

        if (item is null)
        {
            return Result.Failure<MovementDomain>(new ResultError("Movement.InvalidItem", "Item not found"));
        }

        var movementType = await dbContext.MovementTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Movement.MovementTypeId, cancellationToken);

        if (movementType is null)
        {
            return Result.Failure<MovementDomain>(new ResultError("Movement.InvalidMovementType", "Movement type not found"));
        }

        var location = await dbContext.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Movement.LocationId, cancellationToken);

        var movementResult = MovementDomain.Create(
            item,
            request.Movement.Quantity,
            movementType,
            location,
            request.Movement.MovementDate);

        if (movementResult.IsFailure)
        {
            return Result.Failure<MovementDomain>(movementResult.Error);
        }

        await dbContext.Movements
            .AddAsync(movementResult.Value, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(movementResult.Value);
    }
}
