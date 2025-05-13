using Inventory.Domain.Entities;

namespace Inventory.API.Types;

public class ItemType : ObjectType<Item>
{
    protected override void Configure(IObjectTypeDescriptor<Item> descriptor) 
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(i => i.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(i => i.Quantity)
            .Type<NonNullType<IntType>>();

        descriptor.Field(i => i.ExpiryDate)
            .Type<DateType>();

        descriptor.Field(i => i.ProductVariantId)
            .Type<NonNullType<IdType>>();

        descriptor.Field(i => i.Location)
            .Type<NonNullType<LocationType>>();
    }
}
