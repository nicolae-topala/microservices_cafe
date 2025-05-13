using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Inventory.Domain.Entities;

public class LocationType : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    private LocationType() { }

    private LocationType(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<LocationType> Create(string name, string description)
    {
        return Result.Success(new LocationType(name, description));
    }
}
