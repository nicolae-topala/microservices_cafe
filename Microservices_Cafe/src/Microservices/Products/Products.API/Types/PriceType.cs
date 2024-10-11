using Shared.Enums;
using Shared.ValueObjects;

namespace Products.API.Types;

public class PriceType : ObjectType<Price>
{
    protected override void Configure(IObjectTypeDescriptor<Price> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(p => p.Amount)
            .Type<NonNullType<DecimalType>>();

        descriptor.Field(p => p.Currency)
            .Type<NonNullType<EnumType<CurrencyEnum>>>();
    }
}