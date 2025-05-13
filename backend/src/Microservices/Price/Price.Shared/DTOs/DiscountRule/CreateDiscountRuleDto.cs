namespace Price.Shared.DTOs.DiscountRule;

public record CreateDiscountRuleDto(
    Guid? ProductVariantId,
    Guid? ProductCategoryId,
    decimal DiscountPercentage,
    string Condition,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    Guid ChannelId);