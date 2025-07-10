using Mapster;
using Products.Domain.Entities;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Mappings;

public class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Category, CategoryDocument>();
    }
}
