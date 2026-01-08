using Products.Domain.Events.Product;
using Products.Domain.Events.ProductVariant;
using Products.Shared.DTOs.Product;
using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Primitives;
using Shared.ValueObjects;

namespace Products.Domain.Entities;

public sealed class Product : AggregateRoot
{
    private readonly List<Category> _categories = [];
    private readonly List<ProductVariant> _variants = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsVisible { get; private set; }
    public bool IsInStock { get; private set; }
    public ProductType Type { get; private set; }
    public IReadOnlyCollection<Category> Categories => _categories;
    public IReadOnlyCollection<ProductVariant> Variants => _variants;

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

        RaiseDomainEvent(new ProductCreatedEvent(Id));
    }

    public static Result<Product> Create(
        string name,
        string description,
        ProductType type,
        List<Category> categories,
        decimal defaultPrice,
        Currency defaultCurrency)
    {
        var product = new Product(name, description, type, categories);

        var defaultVariantResult = product.CreateDefaultVariant(defaultPrice, defaultCurrency);
        if (defaultVariantResult.IsFailure)
        {
            return Result.Failure<Product>(defaultVariantResult.Error);
        }

        return Result.Success(product);
    }

    private Result<ProductVariant> CreateDefaultVariant(decimal price, Currency currency) =>
        AddVariant(price, currency, []);

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

        RaiseDomainEvent(new ProductUpdatedEvent(Id));
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
            this,
            price,
            currency,
            variantAttributes);

        if (variant.IsFailure)
        {
            return Result.Failure<ProductVariant>(variant.Error);
        }

        _variants.Add(variant.Value);
        RaiseDomainEvent(new ProductVariantCreatedEvent(Id, variant.Value.Id));

        return Result.Success(variant.Value);
    }

    public Result<ProductVariant> UpdateVariant(Guid variantId,
        decimal? price = null,
        Currency? currency = null,
        bool? isVisible = null,
        bool? isInStock = null)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == variantId);

        if (variant is null)
        {
            return Result.Failure<ProductVariant>(new ResultError("Variant.NotFound", "Product variant with the specified ID does not exist."));
        }

        if (price.HasValue || currency.HasValue)
        {
            var priceResult = variant.AddOrUpdatePrice(price, currency);
            if (priceResult.IsFailure)
            {
                return Result.Failure<ProductVariant>(priceResult.Error);
            }
        }

        if (isVisible.HasValue)
        {
            variant.SetVisibility(isVisible.Value);
        }

        if (isInStock.HasValue)
        {
            variant.UpdateStockStatus(isInStock.Value);
        }

        RaiseDomainEvent(new ProductVariantUpdatedEvent(Id, variant.Id));
        return Result.Success(variant);
    }

    public void NotifyVariantUpdated(Guid variantId)
    {
        RaiseDomainEvent(new ProductVariantUpdatedEvent(Id, variantId));
    }

    public Result RemoveVariant(Guid variantId)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == variantId);

        if (variant is null)
        {
            return Result.Failure(new ResultError("Variant.NotFound", "Product variant with the specified ID does not exist."));
        }

        _variants.Remove(variant);
        return Result.Success();
    }

    public Result<ProductVariant> GetVariant(Guid variantId)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == variantId);

        if (variant is null)
        {
            return Result.Failure<ProductVariant>(new ResultError("Variant.NotFound", "Product variant with the specified ID does not exist."));
        }

        return Result.Success(variant);
    }

    public Result UpdateAllVariantsStockStatus(bool isInStock)
    {
        foreach (var variant in _variants)
        {
            variant.UpdateStockStatus(isInStock);
        }

        IsInStock = isInStock;
        return Result.Success();
    }

    public bool HasVariants() => _variants.Count != 0;

    public Result<Price> GetLowestPrice()
    {
        if (!HasVariants())
        {
            return Result.Failure<Price>(new ResultError("Product.NoVariants", "Product has no variants to determine price."));
        }

        var lowestPriceVariant = _variants.MinBy(v => v.Price.Amount);
        if (lowestPriceVariant is null)
        {
            return Result.Failure<Price>(new ResultError("Product.NoVariants", "Product has no variants to determine price."));
        }

        return Result.Success(lowestPriceVariant.Price);
    }

    public void SetVisibility(bool isVisible)
    {
        IsVisible = isVisible;
        RaiseDomainEvent(new ProductUpdatedEvent(Id));
    }
}
