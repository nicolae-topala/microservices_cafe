using Shared.BuildingBlocks.Result;
using Shared.Primitives;
using PriceVO = Shared.ValueObjects.Price;

namespace Price.Domain.Entities;

public class ProductPrice : AggregateRoot
{
    public Guid ProductVariantId { get; private set; }
    public PriceVO Price { get; private set; }
    public DateTime EffectiveFrom { get; private set; }
    public DateTime EffectiveTo { get; private set; }
    public Guid ChannelId { get; private set; }
    public Channel Channel { get; private set; }

    private ProductPrice() { }

    private ProductPrice(Guid productVariantId, PriceVO price, DateTime effectiveFrom, DateTime effectiveTo, Channel channel)
    {
        ProductVariantId = productVariantId;
        Price = price;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        ChannelId = channel.Id;
    }
    public static Result<ProductPrice> Create(
        Guid productVariantId,
        PriceVO price,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        Channel channel)
    {
        if (productVariantId == Guid.Empty)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidProductVariant", "Product variant ID is required."));
        }

        if (price == null)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidPrice", "Price is required."));
        }

        if (channel == null)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidChannel", "Channel is required."));
        }

        if (effectiveFrom > effectiveTo)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidDateRange",
                    "Effective start date must be before or equal to effective end date."));
        }

        var productPrice = new ProductPrice(
            productVariantId,
            price,
            effectiveFrom,
            effectiveTo,
            channel);

        return Result.Success(productPrice);
    }

    public Result<ProductPrice> Update(
        Guid? productVariantId,
        PriceVO? price,
        DateTime? effectiveFrom,
        DateTime? effectiveTo,
        Channel? channel)
    {
        if (productVariantId.HasValue)
        {
            ProductVariantId = productVariantId.Value;
        }

        if (price != null)
        {
            var result = UpdatePrice(price);
            if (result.IsFailure)
            {
                return Result.Failure<ProductPrice>(result.Error);
            }
        }

        if (effectiveFrom.HasValue && effectiveTo.HasValue)
        {
            var result = UpdateDateRange(effectiveFrom.Value, effectiveTo.Value);
            if (result.IsFailure)
            {
                return Result.Failure<ProductPrice>(result.Error);
            }
        }

        if (channel != null)
        {
            ChannelId = channel.Id;
        }

        return Result.Success(this);
    }

    public Result<ProductPrice> UpdatePrice(PriceVO newPrice)
    {
        if (newPrice == null)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidPrice", "Price is required."));
        }

        Price = newPrice;
        return Result.Success(this);
    }

    public Result<ProductPrice> UpdateDateRange(DateTime newEffectiveFrom, DateTime newEffectiveTo)
    {
        if (newEffectiveFrom > newEffectiveTo)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidDateRange",
                    "Effective start date must be before or equal to effective end date."));
        }

        EffectiveFrom = newEffectiveFrom;
        EffectiveTo = newEffectiveTo;
        return Result.Success(this);
    }

    public bool IsExpired() => DateTime.UtcNow > EffectiveTo;

    public bool IsEffective() => DateTime.UtcNow >= EffectiveFrom && DateTime.UtcNow <= EffectiveTo ;

    public decimal CalculateFinalPrice(IEnumerable<DiscountRule> applicableDiscounts)
    {
        if (!IsEffective())
        {
            return Price.Amount;
        }

        var finalPrice = Price.Amount;

        foreach (var discount in applicableDiscounts.Where(d => d.IsEffective()))
        {
            finalPrice = discount.ApplyDiscount(finalPrice);
        }

        return finalPrice;
    }

    public Result<ProductPrice> ApplyPriceIncrease(decimal percentageIncrease)
    {
        if (percentageIncrease < 0)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidPercentage", "Percentage increase cannot be negative."));
        }

        var newAmount = Price.Amount * (1 + (percentageIncrease / 100));
        var newPriceResult = PriceVO.Create(newAmount, Price.Currency);

        if (newPriceResult.IsFailure)
        {
            return Result.Failure<ProductPrice>(newPriceResult.Error);
        }

        Price = newPriceResult.Value;
        return Result.Success(this);
    }

    public Result<ProductPrice> ApplyPriceDecrease(decimal percentageDecrease)
    {
        if (percentageDecrease < 0 || percentageDecrease > 100)
        {
            return Result.Failure<ProductPrice>(
                new ResultError("ProductPrice.InvalidPercentage",
                    "Percentage decrease must be between 0 and 100."));
        }

        var newAmount = Price.Amount * (1 - (percentageDecrease / 100));
        var newPriceResult = PriceVO.Create(newAmount, Price.Currency);

        if (newPriceResult.IsFailure)
        {
            return Result.Failure<ProductPrice>(newPriceResult.Error);
        }

        Price = newPriceResult.Value;
        return Result.Success(this);
    }
}
