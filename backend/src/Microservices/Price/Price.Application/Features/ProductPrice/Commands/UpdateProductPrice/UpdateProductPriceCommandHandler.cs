using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Price.Domain.Entities;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Price.Application.Features.ProductPrice.Commands.UpdateProductPrice;

public class UpdateProductPriceCommandHandler(IPriceDbContext dbContext)
    : IResultCommandHandler<UpdateProductPriceCommand>
{
    public async Task<Result> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
    {
        var productPrice = await dbContext.ProductPrices
            .FirstOrDefaultAsync(x => x.Id == request.ProductPrice.ProductPriceId, cancellationToken);

        if (productPrice is null)
        {
            return Result.Failure(new ResultError("ProductPrice.NotFound", "Product price not found"));
        }

        Channel? channel = null;
        if (request.ProductPrice.ChannelId.HasValue)
        {
            channel = await dbContext.Channels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProductPrice.ChannelId, cancellationToken);

            if (channel is null)
            {
                return Result.Failure(new ResultError("ProductPrice.InvalidChannel", "Channel not found"));
            }
        }

        var productPriceResult = productPrice.Update(
            request.ProductPrice.ProductVariantId,
            request.ProductPrice.Price,
            request.ProductPrice.EffectiveFrom,
            request.ProductPrice.EffectiveTo,
            channel);

        if (productPriceResult.IsFailure)
        {
            return Result.Failure(productPriceResult.Error);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
