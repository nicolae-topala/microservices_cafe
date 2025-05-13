using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Price.Domain.Entities;

public class Channel : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    private Channel() { }

    private Channel(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<Channel> Create(string name, string description)
    {
        return Result.Success(new Channel(name, description));
    }
}
