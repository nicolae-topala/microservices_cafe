using Shared.BuildingBlocks.Result;
using Shared.Errors;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class ProductImage : BaseEntity
{
    public string ImageUrl { get; private set; }
    public string? AltText { get; private set; }
    public int SortOrder { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; }

    private ProductImage() { }

    private ProductImage(
        ProductVariant productVariant,
        string imageUrl,
        string? altText,
        int sortOrder)
    {
        ProductVariant = productVariant;
        ProductVariantId = productVariant.Id;
        ImageUrl = imageUrl;
        AltText = altText;
        SortOrder = sortOrder;
    }

    public static Result<ProductImage> Create(
        ProductVariant productVariant,
        string imageUrl,
        string? altText = null,
        int sortOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Result.Failure<ProductImage>(new ResultError("Image.InvalidUrl", "Image URL cannot be empty."));
        }

        if (sortOrder < 0)
        {
            return Result.Failure<ProductImage>(new ResultError("Image.InvalidSortOrder", "Sort order cannot be negative."));
        }

        return Result.Success(new ProductImage(productVariant, imageUrl, altText, sortOrder));
    }

    public void UpdateSortOrder(int sortOrder)
    {
        if (sortOrder >= 0)
        {
            SortOrder = sortOrder;
        }
    }

    public void UpdateAltText(string? altText)
    {
        AltText = altText;
    }

    public Result UpdateImageUrl(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Result.Failure(new ResultError("Image.InvalidUrl", "Image URL cannot be empty."));
        }

        ImageUrl = imageUrl;
        return Result.Success();
    }

    public bool IsMainImage() => SortOrder == 0;

    public void SetAsMainImage()
    {
        SortOrder = 0;
    }

    public Result Validate()
    {
        if (string.IsNullOrWhiteSpace(ImageUrl))
        {
            return Result.Failure(new ResultError("Image.InvalidUrl", "Image URL cannot be empty."));
        }

        if (SortOrder < 0)
        {
            return Result.Failure(new ResultError("Image.InvalidSortOrder", "Sort order cannot be negative."));
        }

        return Result.Success();
    }

    public string GetFormattedImageDescription()
    {
        return string.IsNullOrWhiteSpace(AltText)
            ? $"Image {SortOrder + 1}"
            : AltText;
    }

    public Result<ProductImage> Clone(ProductVariant newProductVariant)
    {
        return Create(
            newProductVariant,
            ImageUrl,
            AltText,
            SortOrder);
    }
}