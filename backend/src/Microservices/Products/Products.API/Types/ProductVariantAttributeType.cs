using Products.Domain.ValueObjects;
using Products.Shared.Enums;
using Shared.Enums;

namespace Products.API.Types;

public class ProductVariantAttributeType : ObjectType<ProductVariantAttribute>
{
    protected override void Configure(IObjectTypeDescriptor<ProductVariantAttribute> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(pva => pva.Key)
            .Type<NonNullType<EnumType<ProductVariantTypes>>>();

        descriptor.Field(pva => pva.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(pva => pva.Type)
            .Type<EnumType<MeasurementType>>();

        descriptor.Field(pva => pva.Value)
            .Type<FloatType>();
    }
}
