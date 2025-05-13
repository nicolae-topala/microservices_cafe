using LocationDomain = Inventory.Domain.Entities.Location;

namespace Inventory.API.Types;

public class LocationType : ObjectType<LocationDomain>
{
    protected override void Configure(IObjectTypeDescriptor<LocationDomain> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(l => l.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(l => l.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(l => l.Address)
            .Type<NonNullType<AddressType>>();

        descriptor.Field(l => l.LocationType)
            .Type<NonNullType<LocationCategoryType>>();
    }
}

