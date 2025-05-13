using PriceVO = Shared.ValueObjects.Price;

namespace Price.Shared.DTOs.ProductPrice;

public record UpdateProductPriceDto(
    Guid ProductPriceId,
    Guid? ProductVariantId,
    PriceVO? Price,
    Guid? ChannelId,
    DateTime? EffectiveFrom,
    DateTime? EffectiveTo);
