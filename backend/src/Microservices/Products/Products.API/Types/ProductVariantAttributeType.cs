using Products.Domain.Entities;

namespace Products.API.Types;

public class ProductVariantAttributeType : ObjectType<ProductVariantAttribute>
{
    protected override void Configure(IObjectTypeDescriptor<ProductVariantAttribute> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(pva => pva.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(pva => pva.Value)
            .Type<StringType>();

        descriptor.Field(c => c.AttributeDefinition)
            .Type<VariantAttributeDefinitionType>();

        descriptor.Field(pva => pva.UnitsOfMeasure)
            .Type<UnitsOfMeasureType>();
    }
}