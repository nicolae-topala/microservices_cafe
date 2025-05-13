using Shared.ValueObjects;

namespace Inventory.Shared.DTOs.Location;

public record UpdateLocationDto(
    Guid LocationId,
    string? Name = null,
    Address? Address = null,
    Guid? LocationTypeId = null
);
