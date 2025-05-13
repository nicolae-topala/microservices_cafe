using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using ItemDomain = Inventory.Domain.Entities.Item;

namespace Inventory.Application.Features.Item.Commands.CreateItem;

public class CreateItemCommandHandler(IInventoryDbContext dbContext)
    : IResultCommandHandler<CreateItemCommand, ItemDomain>
{
    public async Task<Result<ItemDomain>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var location = await dbContext.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Item.LocationId, cancellationToken);

        if (location is null)
        {
            return Result.Failure<ItemDomain>(new ResultError("Item.InvalidLocation", "Location not found"));
        }

        var itemResult = ItemDomain.Create(
        request.Item.Quantity,
        request.Item.ProductVariantId,
        location,
        request.Item.ExpiryDate);

        if (itemResult.IsFailure)
        {
            return Result.Failure<ItemDomain>(itemResult.Error);
        }

        await dbContext.Items
            .AddAsync(itemResult.Value, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(itemResult.Value);
    }
}
