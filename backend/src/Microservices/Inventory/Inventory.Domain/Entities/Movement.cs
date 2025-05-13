using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Inventory.Domain.Entities;

public class Movement : BaseEntity
{
    public int Quantity { get; private set; }
    public DateTime MovementDate { get; private set; }
    public Guid ItemId { get; private set; }
    public Guid MovementTypeId { get; private set; }
    public Guid? LocationId { get; private set; }
    public Item Item { get; private set; }
    public MovementType MovementType { get; private set; }
    public Location? Location { get; private set; }

    private Movement() { }

    private Movement(
        Item item,
        int quantity,
        DateTime movementDate,
        MovementType movementType,
        Location? location = null)
    {
        Item = item;
        Quantity = quantity;
        MovementDate = movementDate;
        MovementTypeId = movementType.Id;
        LocationId = location.Id;
    }


    public static Result<Movement> Create(
        Item item,
        int quantity,
        MovementType movementType,
        Location? location = null,
        DateTime? movementDate = null)
    {
        if (item == null)
        {
            return Result.Failure<Movement>(new ResultError("Movement.InvalidItem", "Item cannot be null."));
        }

        if (quantity == 0)
        {
            return Result.Failure<Movement>(new ResultError("Movement.InvalidQuantity", "Quantity cannot be zero."));
        }

        if (movementType == null)
        {
            return Result.Failure<Movement>(new ResultError("Movement.InvalidMovementType", "Movement type cannot be null."));
        }

        var actualMovementDate = movementDate ?? DateTime.UtcNow;

        var movement = new Movement(item, quantity, actualMovementDate, movementType, location);

        return Result.Success(movement);
    }

    public Result<Movement> Update(
        int? quantity,
        MovementType? movementType,
        Location? location,
        DateTime? movementDate)
    {
        if (quantity.HasValue)
        {
            UpdateQuantity(quantity.Value);
        }

        if (movementType != null)
        {
            MovementType = movementType;
            MovementTypeId = movementType.Id;
        }

        if (location != null)
        {
            LocationId = location.Id;
        }

        if (movementDate.HasValue)
        {
            MovementDate = movementDate.Value;
        }

        return Result.Success(this);
    }

    public Result<Movement> UpdateQuantity(int newQuantity)
    {

        if (newQuantity == 0)
        {
            return Result.Failure<Movement>(new ResultError("Movement.InvalidQuantity", "Quantity cannot be zero."));
        }

        Quantity = newQuantity;
        return Result.Success(this);
    }
}
