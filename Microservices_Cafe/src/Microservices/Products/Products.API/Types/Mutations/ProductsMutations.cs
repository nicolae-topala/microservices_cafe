using MapsterMapper;
using MediatR;
using Products.Application.Features.Products.Commands.CreateProduct;
using Products.Shared.DTOs;
using Shared.Models;

namespace Products.API.Types.Mutations;

[MutationType]
public class ProductsMutations()
{
    public async Task<Response<ProductDto>> CreateProduct([Service] ISender sender, [Service] IMapper mapper, CreateProductDto product)
    {
        var result = await sender.Send(new CreateProductCommand(product));
        return mapper.Map<Response<ProductDto>>(result);
    }
}

