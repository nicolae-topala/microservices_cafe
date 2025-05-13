using Products.Domain.Entities;

namespace Products.API.Types;

public class VariantAttributeDefinitionType : ObjectType<VariantAttributeDefinition>
{
    protected override void Configure(IObjectTypeDescriptor<VariantAttributeDefinition> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(vad => vad.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(vad => vad.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(vad => vad.Description)
            .Type<StringType>();
    }
}