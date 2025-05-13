namespace Inventory.Shared.DTOs.Item;

public record CreateItemDto(
    Guid ProductVariantId,
    Guid LocationId,
    int Quantity,
    DateOnly? ExpiryDate
);
