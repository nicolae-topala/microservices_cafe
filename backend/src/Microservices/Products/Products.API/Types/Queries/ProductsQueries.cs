using Elastic.Clients.Elasticsearch;
using MediatR;
using Products.Application.Features.Products.Queries.GetProducts;
using Products.Application.Features.Products.Queries.SearchProductsByName;
using Products.Application.Features.Products.Queries.SearchProductsWithFilters;
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

    public Task<IEnumerable<ProductDocument>> SearchProductsByName(
        ISender sender,
        string productName,
        int skip,
        int take,
        CancellationToken cancellationToken) =>
            sender.Send(new SearchProductsByNameQuery(productName, skip, take), cancellationToken);

    public Task<SearchProductsWithFiltersResponse> SearchProductsWithFilters(
        ISender sender,
        int skip,
        int take,
        SortBy sortBy,
        string? productName = null,
        List<Guid>? categoryIds = null,
        double? minPrice = null,
        double? maxPrice = null,
        bool? inStockOnly = null,
        List<Guid>? variantAttributesIds = null,
        SortOrder? sortOrder = null,
        CancellationToken cancellationToken = default) =>
            sender.Send(new SearchProductsWithFiltersQuery(
                skip,
                take,
                sortBy,
                productName,
                categoryIds,
                minPrice,
                maxPrice,
                inStockOnly,
                variantAttributesIds,
                sortOrder),
                cancellationToken);
}