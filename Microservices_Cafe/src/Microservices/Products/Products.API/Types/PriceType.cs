using Products.Shared.DTOs;
using Shared.Enums;

namespace Products.API.Types;

public class PriceType : ObjectType<PriceDto>
{
    protected override void Configure(IObjectTypeDescriptor<PriceDto> descriptor)
    {
        descriptor.Field(p => p.Ammount)
            .Type<NonNullType<DecimalType>>();

        descriptor.Field(p => p.Currency)
            .Type<NonNullType<EnumType<CurrencyEnum>>>();
    }
}

