using MicroservicesCafe.Shared.Enums;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Products.API.Types;

public class PriceType : ObjectType<Price>
{
    protected override void Configure(IObjectTypeDescriptor<Price> descriptor)
    {
        descriptor.Field(p => p.Ammount)
            .Type<NonNullType<DecimalType>>();

        descriptor.Field(p => p.Currency)
            .Type<NonNullType<EnumType<CurrencyEnum>>>();

        descriptor.Field(x => x.GetAtomicValues()).Ignore();
        descriptor.Field(x => x.ToString()).Ignore();
    }
}

