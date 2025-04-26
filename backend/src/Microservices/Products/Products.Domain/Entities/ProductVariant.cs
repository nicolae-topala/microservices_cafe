using Products.Domain.ValueObjects;
using Products.Shared.Enums;
using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Errors;
using Shared.Primitives;
using Shared.ValueObjects;

namespace Products.Domain.Entities;

public sealed class ProductVariant : BaseEntity
{
    private readonly List<string>? _ingredients = [];
    private readonly List<ProductImage> _variantImages = [];
    private readonly List<ProductVariantAttribute> _variantAttributes= [];

    public Guid ProductId { get; private set; }
    public Price Price { get; private set; }
    public bool IsInStock { get; private set; }
    public bool IsVisible { get; private set; }
    public IReadOnlyCollection<string>? Ingredients => _ingredients;
    public IReadOnlyCollection<ProductImage> Images => _variantImages;
    public IReadOnlyCollection<ProductVariantAttribute> VariantAttributes => _variantAttributes;

    private ProductVariant() { }

    private ProductVariant(
        Guid productId,
        Price price,
        List<ProductVariantAttribute> variantAttributes)
    {
        ProductId = productId;
        _variantAttributes = variantAttributes;
        Price = price;
        IsInStock = false;
        IsVisible = false;
    }

    public static Result<ProductVariant> Create(
        Guid productId,
        decimal price,
        Currency currency,
        List<ProductVariantAttribute> variantAttributes)
    {
        var priceResult = Price.Create(price, currency);
        if (priceResult.IsFailure)
        {
            return Result.Failure<ProductVariant>(priceResult.Error);
        }

        return Result.Success(new ProductVariant(productId, priceResult.Value, variantAttributes));
    }

    public void UpdateStockStatus(bool isInStock)
    {
        IsInStock = isInStock;
    }

    public Result AddImage(string imageUrl, string? altText = null, int sortOrder = 0)
    {
        var image = ProductImage.Create(Id, imageUrl, altText, sortOrder);
        if (image.IsFailure)
        {
            return Result.Failure<ProductVariant>(image.Error);
        }

        _variantImages.Add(image.Value);
        return Result.Success();
    }

    public Result AddOrUpdatePrice(decimal? price = null, Currency? currency = null)
    {
        if (price is null && currency is null)
        {
            return Result.Failure<Product>(CommonErrors.NullValue);
        }

        decimal actualPrice = price ?? Price.Amount;
        Currency actualCurrency = currency ?? Price.Currency;

        var priceResult = Price.Create(actualPrice, actualCurrency);
        if (priceResult.IsFailure)
        {
            return Result.Failure<Product>(priceResult.Error);
        }

        Price = priceResult.Value;
        return Result.Success();
    }

    public Result AddOrUpdateVariantAttribute(ProductVariantAttribute variantAttribute)
    {
        var existingAttribute = _variantAttributes.FirstOrDefault(x => 
            x.Key == variantAttribute.Key 
            && x.Name == variantAttribute.Name);

        if (existingAttribute is not null)
        {
            _variantAttributes.Remove(existingAttribute);
        }

        _variantAttributes.Add(variantAttribute);
        return Result.Success();
    }

    public void RemoveVariantAttribute(ProductVariantTypes type, string name)
    {
        _variantAttributes.RemoveAll(x => x.Key == type && x.Name == name);
    }
}
