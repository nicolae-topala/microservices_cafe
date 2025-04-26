using Products.Domain.Entities;

namespace Products.API.Types;

public class CategoryType : ObjectType<Category>
{
    protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(c => c.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(c => c.Name)
            .Type<NonNullType<StringType>>();

        descriptor.Field(c => c.Products)
            .Type<NonNullType<ListType<ProductType>>>();

        descriptor.Field(c => c.ParentCategory)
            .Type<CategoryType>();

        descriptor.Field(c => c.SubCategories)
            .Type<ListType<CategoryType>>();
    }
}