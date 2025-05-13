namespace Inventory.Shared.DTOs.Movement;

public record CreateMovementDto(
    Guid ItemId,
    Guid MovementTypeId,
    Guid LocationId,
    int Quantity,
    DateTime MovementDate);
