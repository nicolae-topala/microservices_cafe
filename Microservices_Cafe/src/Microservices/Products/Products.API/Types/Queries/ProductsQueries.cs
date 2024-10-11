using MediatR;
using Products.Application.Features.Products.Queries.GetProducts;
using Products.Domain.Entities;

namespace Products.API.Types.Queries;

[QueryType]
public class ProductsQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<Product>> GetProducts([Service] ISender sender)
    {
        var result = await sender.Send(new GetProductsQuery());
        return result.Value;
    }
}