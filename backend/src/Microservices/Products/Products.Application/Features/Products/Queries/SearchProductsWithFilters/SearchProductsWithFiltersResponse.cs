using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Features.Products.Queries.SearchProductsWithFilters;

public record SearchProductsWithFiltersResponse(
    IEnumerable<ProductDocument> Products,
    long TotalCount,
    SearchFilters? Filters = null);

public record SearchFilters(
    List<CategoryFilter> Categories,
    PriceRangeFilter PriceRange,
    List<AttributeFilter> Attributes);

public record CategoryFilter(
    Guid Id,
    string Name,
    int Count);

public record PriceRangeFilter(
    double? MinPrice,
    double? MaxPrice,
    List<PriceRangeBucket> Buckets);

public record PriceRangeBucket(
    decimal From,
    decimal To,
    int Count,
    string Label);

public record AttributeFilter(
    Guid Id,
    string AttributeName,
    List<AttributeValueFilter> Values);

public record AttributeValueFilter(
    string Value,
    int Count,
    string? UnitsOfMeasure = null);