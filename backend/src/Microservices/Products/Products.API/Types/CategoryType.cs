using Products.Domain.Entities;

namespace Products.API.Types;

public class CategoryType : ObjectType<Category>
{
    protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(p => p.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(p => p.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(p => p.Products)
            .Type<NonNullType<ListType<ProductType>>>();

        descriptor.Field(p => p.ParentCategory)
            .Type<CategoryType>();

        descriptor.Field(p => p.SubCategories)
            .Type<ListType<CategoryType>>();
    }
}