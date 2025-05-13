using Inventory.Domain.Entities;

namespace Inventory.API.Types;

public class MovementType : ObjectType<Movement>
{
    protected override void Configure(IObjectTypeDescriptor<Movement> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(m => m.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(m => m.Quantity)
            .Type<NonNullType<IntType>>();

        descriptor.Field(m => m.MovementDate)
            .Type<NonNullType<DateTimeType>>();

        descriptor.Field(m => m.Item)
            .Type<NonNullType<ItemType>>();

        descriptor.Field(m => m.Location)
            .Type<NonNullType<LocationType>>();

        descriptor.Field(m => m.MovementType)
            .Type<NonNullType<MovementCategoryType>>();
    }
}
