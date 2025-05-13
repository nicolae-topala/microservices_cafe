using Shared.BuildingBlocks.Result;
using Shared.Primitives;
using Shared.ValueObjects;

namespace Inventory.Domain.Entities;

public class Location : AggregateRoot
{
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Guid LocationTypeId { get; set; }
    public LocationType LocationType { get; set; }

    private Location() { }

    private Location(string name, Address address, LocationType locationType)
    {
        Name = name;
        Address = address;
        LocationTypeId = locationType.Id;
    }

    public static Result<Location> Create(string name, Address address, LocationType locationType)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Location>(new ResultError("Location.InvalidName", "Location name cannot be empty."));
        }

        return Result.Success(new Location(name, address, locationType));
    }

    public Result<Location> Update(string? name, Address? address, LocationType? locationType)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            Name = name;
        }

        if (address != null)
        {
            Address = address;
        }

        if (locationType != null)
        {
            LocationTypeId = locationType.Id;
        }

        return Result.Success(this);
    }
}
