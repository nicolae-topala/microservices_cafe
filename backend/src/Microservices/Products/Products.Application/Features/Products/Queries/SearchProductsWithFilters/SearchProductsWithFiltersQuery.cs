using Elastic.Clients.Elasticsearch;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Queries.SearchProductsWithFilters;

public record SearchProductsWithFiltersQuery(
    int Skip,
    int Take,
    SortBy SortBy,

    string? ProductName = null,
    
    // Category filters
    List<Guid>? CategoryIds = null,
    
    // Price range filters
    double? MinPrice = null,
    double? MaxPrice = null,
    
    // Stock filter
    bool? InStockOnly = null,

    // Variant attribute filters
    List<Guid>? VariantAttributesIds = null,

    // Sorting
    SortOrder? SortOrder = SortOrder.Desc)
    : IQuery<SearchProductsWithFiltersResponse>;

public enum SortBy
{
    Name,
    Price,
    Popularity,
}