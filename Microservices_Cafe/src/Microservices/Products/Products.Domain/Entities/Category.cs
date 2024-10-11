using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; }

    private Category() { }

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

    public Result<Category> Edit(string name)
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

        Name = trimmedName;

        return Result.Success(this);
    }
}
