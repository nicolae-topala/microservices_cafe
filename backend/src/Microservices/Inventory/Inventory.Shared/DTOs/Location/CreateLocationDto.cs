using Shared.ValueObjects;

namespace Inventory.Shared.DTOs.Location;

public record CreateLocationDto(
    string Name,
    Address Address,
    Guid LocationTypeId
);
