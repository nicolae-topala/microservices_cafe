using Shared.Enums;

namespace Products.Shared.DTOs.ProductVariant;

public record AddProductVariantAttributeDto(
    Guid ProductVariantId,
    string Name,
    MeasurementType? Type,
    decimal? Value);