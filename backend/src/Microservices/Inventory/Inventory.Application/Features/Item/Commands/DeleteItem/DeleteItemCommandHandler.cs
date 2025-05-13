using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Inventory.Application.Features.Item.Commands.DeleteItem;

public class DeleteItemCommandHandler(IInventoryDbContext dbContext) 
    : IResultCommandHandler<DeleteItemCommand>
{
    public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.Items
            .FirstOrDefaultAsync(x => x.Id == request.ItemId, cancellationToken);

        if (item is null)
        {
            return Result.Failure(new ResultError("Item.NotFound", "Item not found"));
        }

        dbContext.Items.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
