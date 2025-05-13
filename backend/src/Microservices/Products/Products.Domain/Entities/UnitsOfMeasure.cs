using Shared.BuildingBlocks.Result;
using Shared.Errors;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class UnitsOfMeasure : AggregateRoot
{
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }
    public string? Description { get; private set; }

    private UnitsOfMeasure(string name, string abbreviation, string? description)
    {
        Name = name;
        Abbreviation = abbreviation;
        Description = description;
    }

    public static Result<UnitsOfMeasure> Create(string name, string abbreviation, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(abbreviation))
        {
            return Result.Failure<UnitsOfMeasure>(CommonErrors.NullValue);
        }

        return Result.Create(new UnitsOfMeasure(name, abbreviation, description));
    }
}