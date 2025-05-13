using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Price.Application.Features.ProductPrice.Commands.DeleteProductPrice;

public class DeleteProductPriceCommandHandler(IPriceDbContext dbContext)
    : IResultCommandHandler<DeleteProductPriceCommand>
{
    public async Task<Result> Handle(DeleteProductPriceCommand request, CancellationToken cancellationToken)
    {
        var productPrice = await dbContext.ProductPrices
            .FirstOrDefaultAsync(x => x.Id == request.ProductPriceId, cancellationToken);

        if (productPrice is null)
        {
            return Result.Failure(new ResultError("ProductPrice.NotFound", "Product price not found"));
        }

        dbContext.ProductPrices.Remove(productPrice);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
