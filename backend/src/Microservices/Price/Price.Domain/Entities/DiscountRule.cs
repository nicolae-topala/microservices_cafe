using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Price.Domain.Entities;

public class DiscountRule : AggregateRoot
{
    private const decimal _minDiscountPercentage = 0.0m;
    private const decimal _maxDiscountPercentage = 100.0m;

    public Guid? ProductVariantId { get; private set; }
    public Guid? ProductCategoryId { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public string Condition { get; set; }
    public DateTime EffectiveFrom { get; private set; }
    public DateTime EffectiveTo { get; private set; }
    public Guid ChannelId { get; private set; }
    public Channel Channel { get; private set; }

    private DiscountRule() { }

    private DiscountRule(
        decimal discountPercentage,
        string condition,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        Channel channel,
        Guid? productCategoryId = null,
        Guid? productVariantId = null)
    {
        ProductVariantId = productVariantId;
        ProductCategoryId = productCategoryId;
        DiscountPercentage = discountPercentage;
        Condition = condition;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        ChannelId = channel.Id;
    }
    public static Result<DiscountRule> Create(
        decimal discountPercentage,
        string condition,
        DateTime effectiveFrom,
        DateTime effectiveTo,
        Channel channel,
        Guid? productVariantId = null,
        Guid? productCategoryId = null)
    {
        if (channel == null)
        {
            return Result.Failure<DiscountRule>(new ResultError("DiscountRule.InvalidChannel", "Channel is required."));
        }

        if (discountPercentage < _minDiscountPercentage || discountPercentage > _maxDiscountPercentage)
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.InvalidDiscountPercentage",
                    $"Discount percentage must be between {_minDiscountPercentage} and {_maxDiscountPercentage}."));
        }

        if (effectiveFrom > effectiveTo)
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.InvalidDateRange",
                    "Effective start date must be before or equal to effective end date."));
        }

        if (productVariantId == null && productCategoryId == null)
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.NoTarget",
                    "At least one of product variant ID or product category ID must be specified."));
        }

        if (string.IsNullOrWhiteSpace(condition))
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.InvalidCondition", "Condition is required."));
        }

        var discountRule = new DiscountRule(
            discountPercentage,
            condition,
            effectiveFrom,
            effectiveTo,
            channel,
            productCategoryId,
            productVariantId);

        return Result.Success(discountRule);
    }

    public Result<DiscountRule> Update(
        decimal? discountPercentage,
        string? condition,
        DateTime? effectiveFrom,
        DateTime? effectiveTo,
        Channel? channel,
        Guid? productVariantId,
        Guid? productCategoryId)
    {
        if (discountPercentage.HasValue)
        {
            var result = UpdateDiscountPercentage(discountPercentage.Value);
            if (result.IsFailure)
            {
                return Result.Failure<DiscountRule>(result.Error);
            }
        }

        if (condition != null)
        {
            var result = UpdateCondition(condition);
            if (result.IsFailure)
            {
                return Result.Failure<DiscountRule>(result.Error);
            }
        }

        if (effectiveFrom.HasValue && effectiveTo.HasValue)
        {
            var result = UpdateDateRange(effectiveFrom.Value, effectiveTo.Value);
            if (result.IsFailure)
            {
                return Result.Failure<DiscountRule>(result.Error);
            }
        }

        if (productVariantId.HasValue)
        {
            ProductVariantId = productVariantId.Value;
        }

        if (productCategoryId.HasValue)
        {
            ProductCategoryId = productCategoryId.Value;
        }

        if (channel != null)
        {
            ChannelId = channel.Id;
        }

        return Result.Success(this);
    }

    public Result<DiscountRule> UpdateDiscountPercentage(decimal newDiscountPercentage)
    {
        if (newDiscountPercentage < _minDiscountPercentage || newDiscountPercentage > _maxDiscountPercentage)
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.InvalidDiscountPercentage",
                    $"Discount percentage must be between {_minDiscountPercentage} and {_maxDiscountPercentage}."));
        }

        DiscountPercentage = newDiscountPercentage;
        return Result.Success(this);
    }

    public Result<DiscountRule> UpdateDateRange(DateTime newEffectiveFrom, DateTime newEffectiveTo)
    {
        if (newEffectiveFrom > newEffectiveTo)
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.InvalidDateRange",
                    "Effective start date must be before or equal to effective end date."));
        }

        EffectiveFrom = newEffectiveFrom;
        EffectiveTo = newEffectiveTo;
        return Result.Success(this);
    }

    public Result<DiscountRule> UpdateCondition(string newCondition)
    {
        if (string.IsNullOrWhiteSpace(newCondition))
        {
            return Result.Failure<DiscountRule>(
                new ResultError("DiscountRule.InvalidCondition", "Condition is required."));
        }

        Condition = newCondition;
        return Result.Success(this);
    }

    public bool IsExpired() => DateTime.UtcNow > EffectiveTo;

    public bool IsEffective() => DateTime.UtcNow >= EffectiveFrom && DateTime.UtcNow <= EffectiveTo;

    public bool IsApplicableToVariant(Guid productVariantId) =>
        ProductVariantId == productVariantId || ProductVariantId == null;

    public bool IsApplicableToCategory(Guid productCategoryId) =>
        ProductCategoryId == productCategoryId || ProductCategoryId == null;

    public decimal CalculateDiscountAmount(decimal originalPrice)
    {
        return originalPrice * (DiscountPercentage / 100m);
    }

    public decimal ApplyDiscount(decimal originalPrice)
    {
        if (!IsEffective())
        {
            return originalPrice;
        }

        var discountAmount = CalculateDiscountAmount(originalPrice);
        return originalPrice - discountAmount;
    }
}
