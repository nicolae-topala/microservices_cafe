using Inventory.Application.Abstractions;
using LocationDomain = Inventory.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Inventory.Application.Features.Item.Commands.UpdateItem;

public class UpdateItemCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<UpdateItemCommand>
{
    public async Task<Result> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.Items
           .FirstOrDefaultAsync(x => x.Id == request.Item.ItemId, cancellationToken);

        if (item is null)
        {
            return Result.Failure(new ResultError("", ""));
        }

        LocationDomain? location = null;
        if (request.Item.LocationId.HasValue)
        {
            location = await dbContext.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Item.LocationId.Value, cancellationToken);

            if (location is null)
            {
                return Result.Failure(new ResultError("", ""));
            }
        }

        var itemResult = item.Update(
            request.Item.Quantity,
            request.Item.ProductVariantId,
            location,
            request.Item.ExpiryDate);

        if (itemResult.IsFailure)
        {
            return Result.Failure(itemResult.Error);
        }

        await dbContext
            .SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
