using Shared.Enums;
using PriceVO = Shared.ValueObjects.Price;

namespace Price.API.Types;

public class PriceType : ObjectType<PriceVO>
{
    protected override void Configure(IObjectTypeDescriptor<PriceVO> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(p => p.Amount)
            .Type<NonNullType<DecimalType>>();

        descriptor.Field(p => p.Currency)
            .Type<NonNullType<EnumType<Currency>>>();
    }
}