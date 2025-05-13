using Shared.BuildingBlocks.Result;
using Shared.Errors;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class VariantAttributeDefinition : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private VariantAttributeDefinition(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public static Result<VariantAttributeDefinition> Create(
        string name,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<VariantAttributeDefinition>(SizeErrors.EmptyName);
        }

        return Result.Success(new VariantAttributeDefinition(name, description));
    }
}