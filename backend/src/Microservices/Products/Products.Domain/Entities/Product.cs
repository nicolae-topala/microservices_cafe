using Products.Domain.ValueObjects;
using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class Product : AggregateRoot
{
    private readonly List<Category> _categories = [];
    private readonly List<ProductVariant> _variants = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public ProductType Type { get; private set; }
    public IReadOnlyCollection<Category> Categories => _categories;
    public IReadOnlyCollection<ProductVariant> Variants => _variants;
    public bool IsVisible { get; private set; }
    public bool IsInStock { get; private set; }

    private Product() { }

    private Product(
        string name,
        string description,
        ProductType type,
        List<Category> categories) : base()
    {
        Name = name;
        Description = description;
        Type = type;
        _categories.AddRange(categories);
        IsVisible = false;
        IsInStock = false;
    }

    public static Result<Product> Create(
        string name,
        string description,
        ProductType type,
        List<Category> categories)
    {
        var product = new Product(name, description, type, categories);
        return Result.Success(product);
    }

    public Result<Product> Edit(
        string? name = null,
        string? description = null,
        ProductType? type = null,
        List<Category>? categories = null,
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

        if (type.HasValue)
        {
            Type = type.Value;
        }

        if (isVisible.HasValue)
        {
            IsVisible = isVisible.Value;
        }

        if (isInStock.HasValue)
        {
            IsInStock = isInStock.Value;
        }

        return Result.Success(this);
    }

    public Result<Product> AddNewCategory(Category newCategory)
    {
        _categories.Add(newCategory);
        return Result.Success(this);
    }

    public Result<Product> RemoveCategory(Category category)
    {
        _categories.Remove(category);
        return Result.Success(this);
    }

    public Result<Product> RemoveAllCategories()
    {
        _categories.Clear();
        return Result.Success(this);
    }

    public Result<Product> AddRangeCategories(IEnumerable<Category> newCategories)
    {
        _categories.AddRange(newCategories);
        return Result.Success(this);
    }

    public Result<ProductVariant> AddVariant( 
        decimal price,
        Currency currency,
        List<ProductVariantAttribute> variantAttributes)
    {
        var variant = ProductVariant.Create(
            Id,
            price,
            currency,
            variantAttributes);

        if (variant.IsFailure)
        {
            return Result.Failure<ProductVariant>(variant.Error);
        }

        _variants.Add(variant.Value);
        return Result.Success(variant.Value);
    }
}
