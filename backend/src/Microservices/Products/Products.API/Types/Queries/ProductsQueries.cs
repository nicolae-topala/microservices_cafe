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
    public Task<IQueryable<Product>> GetProducts(ISender sender)
    {
        var result = sender.Send(new GetProductsQuery())
            .ContinueWith(t => t.Result.Value);
        return result;
    }
}