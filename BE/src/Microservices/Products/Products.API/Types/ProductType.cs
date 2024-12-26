using Products.Domain.Entities;
using Shared.Enums;

namespace Products.API.Types;

public class ProductType : ObjectType<Product>
{
    protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(p => p.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(p => p.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(p => p.Description)
            .Type<NonNullType<StringType>>();

        descriptor.Field(p => p.Price)
            .Type<NonNullType<PriceType>>();

        descriptor.Field(p => p.Type)
            .Type<NonNullType<EnumType<ProductTypeEnum>>>();

        descriptor.Field(p => p.Ingredients)
            .Type<NonNullType<ListType<StringType>>>();

        descriptor.Field(p => p.Categories)
            .Type<NonNullType<ListType<CategoryType>>>();

        descriptor.Field(p => p.IsVisible)
            .Type<NonNullType<BooleanType>>();

        descriptor.Field(p => p.IsInStock)
            .Type<NonNullType<BooleanType>>();
    }
}
