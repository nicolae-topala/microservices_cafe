using Mapster;

namespace Products.Application.Mappings;

public class ProductsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<Product, ProductDto>();
    }
}
