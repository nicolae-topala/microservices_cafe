using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;
using ProductPriceDomain = Price.Domain.Entities.ProductPrice;

namespace Price.Application.Features.ProductPrice.Commands.CreateProductPrice;

public class CreateProductPriceCommandHandler(IPriceDbContext dbContext)
    : IResultCommandHandler<CreateProductPriceCommand, ProductPriceDomain>
{
    public async Task<Result<ProductPriceDomain>> Handle(CreateProductPriceCommand request, CancellationToken cancellationToken)
    {
        var channel = await dbContext.Channels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ProductPrice.ChannelId, cancellationToken);

        if (channel is null)
        {
            return Result.Failure<ProductPriceDomain>(new ResultError("ProductPrice.InvalidChannel", "Channel not found"));
        }

        var productPriceResult = ProductPriceDomain.Create(
            request.ProductPrice.ProductVariantId,
            request.ProductPrice.Price,
            request.ProductPrice.EffectiveFrom,
            request.ProductPrice.EffectiveTo,
            channel);

        if (productPriceResult.IsFailure)
        {
            return Result.Failure<ProductPriceDomain>(productPriceResult.Error);
        }

        await dbContext.ProductPrices
            .AddAsync(productPriceResult.Value, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(productPriceResult.Value);
    }
}
