using Products.Domain.Entities;

namespace Products.API.Types;

public class ProductVariantType : ObjectType<ProductVariant>
{
    protected override void Configure(IObjectTypeDescriptor<ProductVariant> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(pv => pv.Id)
            .Type<NonNullType<IdType>>();

        descriptor.Field(pv => pv.ProductId)
            .Type<NonNullType<IdType>>();

        descriptor.Field(pv => pv.IsInStock)
            .Type<NonNullType<BooleanType>>();

        descriptor.Field(pv => pv.IsVisible)
            .Type<NonNullType<BooleanType>>();

        descriptor.Field(pv => pv.Price)
            .Type<NonNullType<PriceType>>();

        descriptor.Field(pv => pv.Images)
            .Type<NonNullType<ListType<ProductImageType>>>();

        descriptor.Field(pv => pv.VariantAttributes)
            .Type<NonNullType<ListType<ProductVariantAttributeType>>>();

        descriptor.Field(pv => pv.Ingredients)
            .Type<ListType<StringType>>();
    }
}
