namespace Inventory.Shared.DTOs.Movement;

public record UpdateMovementDto(
    Guid MovementId,
    Guid? ItemId,
    Guid? MovementTypeId,
    Guid? LocationId,
    int? Quantity,
    DateTime? MovementDate);
