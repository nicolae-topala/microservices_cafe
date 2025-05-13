namespace Products.Shared.DTOs.ProductVariant;

public record AddProductVariantAttributeDto(
    Guid ProductVariantId,
    string Value,
    Guid AttributeDefinitionId,
    Guid? UnitsOfMeasureId = null);