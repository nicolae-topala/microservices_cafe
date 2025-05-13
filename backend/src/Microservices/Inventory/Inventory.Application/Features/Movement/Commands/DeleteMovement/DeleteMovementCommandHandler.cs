using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Inventory.Application.Features.Movement.Commands.DeleteMovement;

public class DeleteMovementCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<DeleteMovementCommand>
{
    public async Task<Result> Handle(DeleteMovementCommand request, CancellationToken cancellationToken)
    {
        var movement = await dbContext.Movements
            .FirstOrDefaultAsync(x => x.Id == request.MovementId, cancellationToken);

        if (movement is null)
        {
            return Result.Failure(new ResultError("Movement.NotFound", "Movement not found"));
        }

        dbContext.Movements.Remove(movement);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
