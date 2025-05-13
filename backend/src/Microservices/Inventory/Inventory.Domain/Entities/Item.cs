using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Inventory.Domain.Entities;

public class Item : AggregateRoot
{
    public int Quantity { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public Guid LocationId { get; private set; }
    public Location Location { get; private set; }

    private Item() { }

    private Item(int quantity, Guid productVariantId, Location location, DateOnly? expiryDate)
    {
        Quantity = quantity;
        ProductVariantId = productVariantId;
        LocationId = location.Id;
        ExpiryDate = expiryDate;
    }

    public static Result<Item> Create(int quantity, Guid productVariantId, Location location, DateOnly? expiryDate)
    {
        if (quantity < 0)
        {
            return Result.Failure<Item>(new ResultError("Item.InvalidQuantity", "Quantity cannot be negative"));
        }

        return Result.Success(new Item(quantity, productVariantId, location, expiryDate));
    }


    public Result<Item> Update(int? quantity, Guid? productVariantId, Location? location, DateOnly? expiryDate)
    {
        if (quantity.HasValue)
        {
            UpdateQuantity(quantity.Value);
        }

        if (productVariantId.HasValue)
        {
            ProductVariantId = productVariantId.Value;
        }

        if (location != null)
        {
            LocationId = location.Id;
        }

        if (expiryDate.HasValue)
        {
            ExpiryDate = expiryDate.Value;
        }
        
        return Result.Success(this);
    }

    public Result<Item> UpdateQuantity(int newQuantity)
    {
        if (newQuantity < 0)
        {
            return Result.Failure<Item>(new ResultError("Item.InvalidQuantity", "Quantity cannot be negative"));
        }

        Quantity = newQuantity;
        return Result.Success(this);
    }

    public bool IsExpired()
    {
        return ExpiryDate.HasValue && ExpiryDate.Value <= DateOnly.FromDateTime(DateTime.Today);
    }
}
