using Mapster;
using Products.Domain.Entities;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Mappings;

public class ProductMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<Product, ProductDto>();
        config.NewConfig<Product, ProductDocument>()
            .Map(dest => dest.Categories, src => src.Categories)
            .Map(dest => dest.Variants, src => src.Variants);
    }
}
