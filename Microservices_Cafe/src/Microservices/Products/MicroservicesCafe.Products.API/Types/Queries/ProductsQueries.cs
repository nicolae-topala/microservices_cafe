using HotChocolate.Execution;
using MediatR;
using MicroservicesCafe.Products.Application.Features.Products.Queries.GetProductById;
using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.BuildingBlocks.GraphQL;

namespace MicroservicesCafe.Products.API.Types.Queries;

[QueryType]
public class ProductsQueries()
{
    public async Task<ProductDto> GetProductById([Service] ISender sender, Guid id)
    {
        var result = await sender.Send(new GetProductByIdQuery(id));

        if (result.IsFailure)
        {
            throw new QueryException(GraphQLHelper.TranslateError(result.Error));
        }

        return result.Value;
    }
}