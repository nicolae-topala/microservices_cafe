using Elastic.Clients.Elasticsearch.Aggregations;
using Products.Application.Features.Products.Queries.SearchProductsWithFilters;

namespace Products.Application.Abstractions;

public interface IElasticsearchAggregationMapper
{
    SearchFilters? MapToSearchFilters(IReadOnlyDictionary<string, IAggregate>? aggregations);
}