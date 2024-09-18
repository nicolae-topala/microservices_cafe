using Mapster;
using Products.Domain.Entities;
using Products.Shared.DTOs;

namespace Products.Application.Mappings;

public class ProductsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>();
    }
}
