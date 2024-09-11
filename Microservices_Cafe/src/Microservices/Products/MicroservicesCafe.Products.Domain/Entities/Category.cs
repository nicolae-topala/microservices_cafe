using MicroservicesCafe.Shared.BuildingBlocks.Result;
using MicroservicesCafe.Shared.Primitives;

namespace MicroservicesCafe.Products.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; }

    private Category(string name) : base()
    {
        Name = name;
    }

    public static Result<Category> Create(string name)
    {
        var nameMaxLength = 64;
        var trimmedName = name.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            return Result.Failure<Category>(new Error("", ""));
        }

        if (trimmedName.Length > nameMaxLength)
        {
            return Result.Failure<Category>(new Error("", ""));
        }

        return Result.Success(new Category(trimmedName));
    }
}
