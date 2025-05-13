using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Inventory.Domain.Entities;

public class MovementType : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private MovementType() { }

    private MovementType(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public static Result<MovementType> Create(string name, string? description = null)
    {
        return Result.Success(new MovementType(name, description));
    }
}
