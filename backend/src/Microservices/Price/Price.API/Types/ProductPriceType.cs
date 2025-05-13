using Price.Domain.Entities;

namespace Price.API.Types;

public class ProductPriceType : ObjectType<ProductPrice>
{
    protected override void Configure(IObjectTypeDescriptor<ProductPrice> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(p => p.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(p => p.ProductVariantId)
            .Type<NonNullType<IdType>>();

        descriptor.Field(p => p.Price)
            .Type<NonNullType<PriceType>>();

        descriptor.Field(p => p.EffectiveFrom)
            .Type<NonNullType<DateTimeType>>();

        descriptor.Field(p => p.EffectiveTo)
            .Type<NonNullType<DateTimeType>>();

        descriptor.Field(p => p.Channel)
            .Type<NonNullType<ChannelType>>();
    }
}
