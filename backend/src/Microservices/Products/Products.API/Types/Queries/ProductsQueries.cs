using MediatR;
using Products.Application.Features.Products.Queries.GetProducts;
using Products.Application.Features.Products.Queries.SearchProductsByName;
using Products.Domain.Entities;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.API.Types.Queries;

[QueryType]
public class ProductsQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Product>> GetProducts(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new GetProductsQuery(), cancellationToken);

    public Task<IEnumerable<ProductDocument>> SearchProductsByName(ISender sender, 
        string productName,
        int? skip,
        int? take,
        List<Guid>? categoryIds,
        decimal? minPrice,
        decimal? maxPrice,
        bool? inStockOnly,
        Dictionary<string, string>? variantAttributes,
        CancellationToken cancellationToken) =>
            sender.Send(new SearchProductsByNameQuery(productName, skip, take, categoryIds, minPrice, maxPrice, inStockOnly, variantAttributes), cancellationToken);
}