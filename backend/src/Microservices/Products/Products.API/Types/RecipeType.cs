using Products.Domain.Entities;

namespace Products.API.Types;

public class RecipeType : ObjectType<Recipe>
{
    protected override void Configure(IObjectTypeDescriptor<Recipe> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(r => r.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(r => r.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(r => r.Description)
            .Type<NonNullType<StringType>>();

        descriptor.Field(r => r.Instructions)
            .Type<NonNullType<StringType>>();

        descriptor.Field(r => r.PreparationTimeInMinutes)
            .Type<NonNullType<IntType>>();

        descriptor.Field(r => r.IsPublished)
            .Type<NonNullType<BooleanType>>();

        descriptor.Field(r => r.ProductVariant)
            .Type<ProductVariantType>();

        descriptor.Field(r => r.Ingredients)
            .Type<NonNullType<ListType<RecipeIngredientType>>>();
    }
}
