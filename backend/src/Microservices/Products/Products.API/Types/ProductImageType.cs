using Products.Domain.Entities;

namespace Products.API.Types;


public class ProductImageType : ObjectType<ProductImage>
{
    protected override void Configure(IObjectTypeDescriptor<ProductImage> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(pi => pi.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(c => c.ProductVariant)
            .Type<ListType<ProductVariantType>>();

        descriptor.Field(pi => pi.ImageUrl)
            .Type<NonNullType<StringType>>();

        descriptor.Field(pi => pi.AltText)
            .Type<StringType>();

        descriptor.Field(pi => pi.SortOrder)
            .Type<NonNullType<IntType>>();
    }
}