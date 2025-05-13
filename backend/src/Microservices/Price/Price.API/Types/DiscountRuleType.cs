using Price.Domain.Entities;

namespace Price.API.Types;

public class DiscountRuleType : ObjectType<DiscountRule>
{
    protected override void Configure(IObjectTypeDescriptor<DiscountRule> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(dr => dr.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(dr => dr.ProductVariantId)
            .Type<IdType>();

        descriptor.Field(dr => dr.ProductCategoryId)
            .Type<IdType>();

        descriptor.Field(dr => dr.Condition)
            .Type<NonNullType<StringType>>();

        descriptor.Field(dr => dr.DiscountPercentage)
            .Type<NonNullType<DecimalType>>();

        descriptor.Field(dr => dr.EffectiveFrom)
            .Type<NonNullType<DateTimeType>>();

        descriptor.Field(dr => dr.EffectiveTo)
            .Type<NonNullType<DateTimeType>>();

        descriptor.Field(dr => dr.Channel)
            .Type<NonNullType<ChannelType>>();
    }
}
