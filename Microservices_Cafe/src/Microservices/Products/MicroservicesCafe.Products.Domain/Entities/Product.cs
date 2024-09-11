using MicroservicesCafe.Shared.BuildingBlocks.Result;
using MicroservicesCafe.Shared.Enums;
using MicroservicesCafe.Shared.Primitives;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Products.Domain.Entities;

public sealed class Product : AggregateRoot
{
    private readonly List<string> _ingredients = [];

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Price Price { get; private set; }
    public ProductTypeEnum Type { get; private set; }
    public IReadOnlyCollection<string> Ingredients => _ingredients;
    public Guid CategoryId { get; private set; }


    private Product()
    {
    }

    private Product(string name, string description, Price price, ProductTypeEnum type, Guid categoryId) : base()
    {
        Name = name;
        Description = description;
        Price = price;
        Type = type;
        CategoryId = categoryId;
    }

    public static Result<Product> Create(string name, string description, Price price, ProductTypeEnum type, Guid categoryId)
    {
        var trimmedName = name.Trim();
        var trimmedDescription = description.Trim();
        var nameMaxLength = 256;
        var descriptionMaxLength = 2000;

        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            return Result.Failure<Product>(new Error("", ""));
        }

        if (trimmedName.Length > nameMaxLength)
        {
            return Result.Failure<Product>(new Error("", ""));
        }

        if (string.IsNullOrWhiteSpace(trimmedDescription))
        {
            return Result.Failure<Product>(new Error("", ""));
        }

        if (trimmedDescription.Length > descriptionMaxLength)
        {
            return Result.Failure<Product>(new Error("", ""));
        }

        var product = new Product(name, description, price, type, categoryId);
        return Result.Success(product);
    }
}
