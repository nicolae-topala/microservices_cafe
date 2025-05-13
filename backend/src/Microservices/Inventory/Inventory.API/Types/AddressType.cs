using Shared.ValueObjects;

namespace Inventory.API.Types;

public class AddressType : ObjectType<Address>
{
    protected override void Configure(IObjectTypeDescriptor<Address> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(a => a.Street)
            .Type<NonNullType<StringType>>();

        descriptor.Field(a => a.City)
            .Type<NonNullType<StringType>>();   

        descriptor.Field(a => a.PostalCode)
            .Type<NonNullType<StringType>>();

        descriptor.Field(a => a.Country)
            .Type<NonNullType<StringType>>();
    }
}
