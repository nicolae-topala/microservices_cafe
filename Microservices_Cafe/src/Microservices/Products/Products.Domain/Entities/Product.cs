using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Primitives;
using Shared.ValueObjects;

namespace Products.Domain.Entities;

public sealed class Product : AggregateRoot
{
    private readonly List<string> _ingredients = [];
    private readonly List<Category> _categories = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public ProductTypeEnum Type { get; private set; }
    public IReadOnlyCollection<string> Ingredients => _ingredients;
    public IReadOnlyCollection<Category> Categories => _categories;
    public bool IsVisible { get; set; }
    public bool IsInStock { get; set; }

    private Product() { }

    private Product(string name, string description, Price price, ProductTypeEnum type, List<Category> categories) : base()
    {
        Name = name;
        Description = description;
        Price = price;
        Type = type;
        _categories.AddRange(categories);
        IsVisible = false;
        IsInStock = false;
    }

    public static Result<Product> Create(string name, string description, Price price, ProductTypeEnum type, List<Category> categories)
    {
        var product = new Product(name, description, price, type, categories);
        return Result.Success(product);
    }

    public Result<Product> Edit(
        string? name = null, 
        string? description = null, 
        decimal? price = null, 
        CurrencyEnum? currency = null, 
        ProductTypeEnum? type = null, 
        Guid? categoryId = null, 
        bool? isVisible = null, 
        bool? isInStock = null)
    {
        if (name is not null)
        {
            Name = name;
        }

        if (description is not null)
        {
            Description = description;
        }

        if (price is not null || currency is not null)
        {
            var priceResult = Price.Edit(price, currency);
            if (priceResult.IsFailure)
            {
                return Result.Failure<Product>(priceResult.Error);
            }
        }

        if (type is not null)
        {
            Type = (ProductTypeEnum)type;
        }

        if (isVisible is not null)
        {
            IsVisible = (bool)isVisible;
        }

        if (isInStock is not null)
        {
            IsInStock = (bool)isInStock;
        }
        return Result.Success(this);
    }
}
