using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.Enums;

namespace MicroservicesCafe.Products.API.Types;

public class ProductType : ObjectType<ProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
    {
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

        descriptor.Field(p => p.CategoryId)
            .Type<NonNullType<IdType>>();
    }
}
