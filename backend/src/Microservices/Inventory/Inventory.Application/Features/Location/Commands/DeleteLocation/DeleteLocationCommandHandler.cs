using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Inventory.Application.Features.Location.Commands.DeleteLocation;

public class DeleteLocationCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<DeleteLocationCommand>
{
    public async Task<Result> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        var location = await dbContext.Locations
            .FirstOrDefaultAsync(x => x.Id == request.LocationId, cancellationToken);

        if (location is null)
        {
            return Result.Failure(new ResultError("Location.NotFound", "Location not found"));
        }

        dbContext.Locations.Remove(location);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
