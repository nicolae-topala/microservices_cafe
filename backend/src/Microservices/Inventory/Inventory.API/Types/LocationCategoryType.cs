using LocationTypeDomain = Inventory.Domain.Entities.LocationType;

namespace Inventory.API.Types;

public class LocationCategoryType : ObjectType<LocationTypeDomain>
{
    protected override void Configure(IObjectTypeDescriptor<LocationTypeDomain> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(lc => lc.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(lc => lc.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(lc => lc.Description)
            .Type<StringType>();
    }
}
