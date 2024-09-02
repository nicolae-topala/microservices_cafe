using Mapster;
using MicroservicesCafe.Products.Domain.Entities;
using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Products.Application.Mappings;

public class ProductsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Product, ProductDto>()
            .ConstructUsing(src => 
                new ProductDto(src.Id, src.Name, src.Description, Price.Create(src.Price.Ammount, src.Price.Currency).Value, src.Type, src.Ingredients.ToList(), src.CategoryId));

        TypeAdapterConfig<ProductDto, Product>
            .NewConfig()
            .ConstructUsing(src => 
                Product.Create(src.Name, src.Description, Price.Create(src.Price.Ammount, src.Price.Currency).Value, src.Type, src.CategoryId).Value);
    }
}

