namespace Inventory.Shared.DTOs.Item;

public record UpdateItemDto(
    Guid ItemId,
    Guid? ProductVariantId,
    Guid? LocationId,
    int? Quantity,
    DateOnly? ExpiryDate
);
