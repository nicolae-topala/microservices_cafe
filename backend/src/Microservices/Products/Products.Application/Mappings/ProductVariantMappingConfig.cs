using Mapster;
using Products.Domain.Entities;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Mappings;

public class ProductVariantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductVariant, ProductVariantDocument>()
            .Map(dest => dest.PriceAmount, src => src.Price.Amount)
            .Map(dest => dest.PriceCurrency, src => src.Price.Currency.ToString())
            .Map(dest => dest.VariantAttributes, src => src.VariantAttributes);
    }
}
