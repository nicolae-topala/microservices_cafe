using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Errors;
using Shared.Primitives;
using Shared.ValueObjects;

namespace Products.Domain.Entities;

public sealed class ProductVariant : BaseEntity
{
    private readonly List<ProductImage> _variantImages = [];
    private readonly List<ProductVariantAttribute> _variantAttributes= [];

    public bool IsInStock { get; private set; }
    public bool IsVisible { get; private set; }
    public Price Price { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public IReadOnlyCollection<ProductImage> Images => _variantImages;
    public IReadOnlyCollection<ProductVariantAttribute> VariantAttributes => _variantAttributes;

    private ProductVariant() { }

    private ProductVariant(
        Product product,
        Price price,
        List<ProductVariantAttribute> variantAttributes)
    {
        Product = product;
        _variantAttributes = variantAttributes;
        Price = price;
        IsInStock = false;
        IsVisible = false;
    }

    public static Result<ProductVariant> Create(
        Product product,
        decimal price,
        Currency currency,
        List<ProductVariantAttribute> variantAttributes)
    {
        var priceResult = Price.Create(price, currency);
        if (priceResult.IsFailure)
        {
            return Result.Failure<ProductVariant>(priceResult.Error);
        }

        var newProductVariant = new ProductVariant(product, priceResult.Value, variantAttributes);
        return Result.Success(newProductVariant);
    }

    public void UpdateStockStatus(bool isInStock)
    {
        IsInStock = isInStock;
    }

    public Result AddImage(string imageUrl, string? altText = null, int sortOrder = 0)
    {
        var image = ProductImage.Create(this, imageUrl, altText, sortOrder);
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

    public Result AddOrUpdateVariantAttribute(VariantAttributeDefinition attributeDefinition, string value, UnitsOfMeasure? unitsOfMeasure = null)
    {
        var existingAttribute = _variantAttributes
          .FirstOrDefault(a => a.AttributeDefinitionId == attributeDefinition.Id);

        if (existingAttribute is not null)
        {
            var updateValueResult = existingAttribute.UpdateValue(value);
            if (updateValueResult.IsFailure)
            {
                return Result.Failure(updateValueResult.Error);
            }

            var updateUnitsResult = existingAttribute.UpdateUnitsOfMeasure(unitsOfMeasure);
            if (updateUnitsResult.IsFailure)
            {
                return Result.Failure(updateUnitsResult.Error);
            }

            Product.NotifyVariantUpdated(Id);
            return Result.Success();
        }

        var attributeResult = ProductVariantAttribute.Create(this, attributeDefinition, value, unitsOfMeasure);
        if (attributeResult.IsFailure)
        {
            return Result.Failure(attributeResult.Error);
        }

        _variantAttributes.Add(attributeResult.Value);
  
        Product.NotifyVariantUpdated(Id);
        return Result.Success();
    }

    public Result RemoveVariantAttribute(Guid attributeDefinitionId)
    {
        var attribute = _variantAttributes.FirstOrDefault(a => a.AttributeDefinitionId == attributeDefinitionId);

        if (attribute is null)
        {
            return Result.Failure(new ResultError("VariantAttribute.NotFound", "Attribute with the specified definition ID does not exist."));
        }

        _variantAttributes.Remove(attribute);
        return Result.Success();
    }

    public Result RemoveImage(Guid imageId)
    {
        var image = _variantImages.FirstOrDefault(i => i.Id == imageId);

        if (image is null)
        {
            return Result.Failure(new ResultError("Image.NotFound", "Image with the specified ID does not exist."));
        }

        _variantImages.Remove(image);
        return Result.Success();
    }

    public Result UpdateImageSortOrder(Guid imageId, int sortOrder)
    {
        var image = _variantImages.FirstOrDefault(i => i.Id == imageId);

        if (image is null)
        {
            return Result.Failure(new ResultError("Image.NotFound", "Image with the specified ID does not exist."));
        }

        image.UpdateSortOrder(sortOrder);
        return Result.Success();
    }

    public string GetVariantDescription()
    {
        if (!_variantAttributes.Any())
        {
            return "Standard variant";
        }

        return string.Join(", ", _variantAttributes.Select(a => $"{a.AttributeDefinition.Name}: {a.Value}{(a.UnitsOfMeasure != null ? " " + a.UnitsOfMeasure.Abbreviation : "")}"));
    }

    public Result<ProductVariant> CloneWithAttributes(List<ProductVariantAttribute> newAttributes)
    {
        return Create(
            Product,
            Price.Amount,
            Price.Currency,
            newAttributes);
    }

    public void SetVisibility(bool isVisible)
    {
        IsVisible = isVisible;
        Product.NotifyVariantUpdated(Id);
    }
}
