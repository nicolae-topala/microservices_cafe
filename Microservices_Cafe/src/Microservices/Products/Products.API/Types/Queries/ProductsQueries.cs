using HotChocolate.Authorization;
using MapsterMapper;
using MediatR;
using Products.Application.Features.Products.Queries.GetProductById;
using Products.Shared.DTOs;
using Shared.Models;

namespace Products.API.Types.Queries;

[QueryType]
[Authorize]
public class ProductsQueries()
{
    public async Task<Response<ProductDto>> GetProductById([Service] ISender sender, [Service]IMapper mapper, Guid id)
    {
        var result = await sender.Send(new GetProductByIdQuery(id));
        return mapper.Map<Response<ProductDto>>(result);
    }
}