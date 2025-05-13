using Products.Domain.Entities;

namespace Products.API.Types;

public class UnitsOfMeasureType : ObjectType<UnitsOfMeasure>
{
    protected override void Configure(IObjectTypeDescriptor<UnitsOfMeasure> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(uom => uom.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(uom => uom.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(uom => uom.Abbreviation)
            .Type<NonNullType<StringType>>();

        descriptor.Field(uom => uom.Description)
            .Type<StringType>();
    } 
}
