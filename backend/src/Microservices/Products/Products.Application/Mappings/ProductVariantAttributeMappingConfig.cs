using Mapster;
using Products.Domain.Entities;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Mappings;

public class ProductVariantAttributeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductVariantAttribute, ProductVariantAttributeDocument>()
            .Map(dest => dest.AttributeName, src => src.AttributeDefinition.Name)
            .Map(dest => dest.UnitsOfMeasureName, src => src.UnitsOfMeasure != null ? src.UnitsOfMeasure.Name : null)
            .Map(dest => dest.UnitsOfMeasureAbbreviation, src => src.UnitsOfMeasure != null ? src.UnitsOfMeasure.Abbreviation : null);
    }
}
