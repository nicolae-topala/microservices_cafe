namespace Price.Shared.DTOs.DiscountRule;

public record UpdateDiscountRuleDto(
    Guid DiscountRuleId,
    Guid? ProductVariantId,
    Guid? ProductCategoryId,
    decimal? DiscountPercentage,
    string? Condition,
    DateTime? EffectiveFrom,
    DateTime? EffectiveTo,
    Guid? ChannelId);