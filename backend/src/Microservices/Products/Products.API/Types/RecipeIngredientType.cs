using Products.Domain.Entities;

namespace Products.API.Types;

public class RecipeIngredientType : ObjectType<RecipeIngredient>
{
    protected override void Configure(IObjectTypeDescriptor<RecipeIngredient> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(ri => ri.Id)
            .Type<NonNullType<IdType>>();
            
        descriptor.Field(ri => ri.Quantity)
            .Type<NonNullType<FloatType>>();
            
        descriptor.Field(ri => ri.Recipe)
            .Type<NonNullType<RecipeType>>();
            
        descriptor.Field(ri => ri.UnitsOfMeasure)
            .Type<UnitsOfMeasureType>();
            
        descriptor.Field(ri => ri.ProductVariant)
            .Type<ProductVariantType>();
    }
}
