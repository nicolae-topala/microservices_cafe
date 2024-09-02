using HotChocolate.Execution;
using MediatR;
using MicroservicesCafe.Products.Application.Features.Products.Commands.CreateProduct;
using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.BuildingBlocks.GraphQL;

namespace MicroservicesCafe.Products.API.Types.Mutations;

[MutationType]
public class ProductsMutations()
{
    public async Task<ProductDto> CreateProduct([Service] ISender sender, CreateProductDto product)
    {
        var result = await sender.Send(new CreateProductCommand(product));

        if (result.IsFailure)
        {
            throw new QueryException(GraphQLHelper.TranslateError(result.Error));
        }

        return result.Value;
    }
}

