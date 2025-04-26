using Shared.BuildingBlocks.Result;
using Shared.Errors;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class ProductImage : BaseEntity
{
    public Guid VariantId { get; private set; }
    public string ImageUrl { get; private set; }
    public string? AltText { get; private set; }
    public int SortOrder { get; private set; } 

    private ProductImage() { }

    private ProductImage(
        Guid variantId,
        string imageUrl,
        string? altText,
        int sortOrder)
    {
        VariantId = variantId;
        ImageUrl = imageUrl;
        AltText = altText;
        SortOrder = sortOrder;
    }

    public static Result<ProductImage> Create(
        Guid variantId,
        string imageUrl,
        string? altText = null,
        int sortOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Result.Failure<ProductImage>(CommonErrors.NullValue);
        }

        return Result.Success(new ProductImage(variantId, imageUrl, altText, sortOrder));
    }

    public void UpdateSortOrder(int sortOrder)
    {
        SortOrder = sortOrder;
    }

    public void UpdateAltText(string? altText)
    {
        AltText = altText;
    }
}