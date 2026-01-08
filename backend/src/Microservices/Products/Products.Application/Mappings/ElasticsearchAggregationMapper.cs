using Elastic.Clients.Elasticsearch.Aggregations;
using Products.Application.Abstractions;
using Products.Application.Features.Products.Queries.SearchProductsWithFilters;
using static Shared.BuildingBlocks.Elasticsearch.ElasticsearchConstants;

namespace Products.Application.Mappings;

public class ElasticsearchAggregationMapper : IElasticsearchAggregationMapper
{
    public SearchFilters? MapToSearchFilters(IReadOnlyDictionary<string, IAggregate>? aggregations)
    {
        if (aggregations == null)
        {
            return null;
        }

        return new SearchFilters(
            ExtractCategories(aggregations),
            ExtractPriceRange(aggregations),
            ExtractAttributes(aggregations)
        );
    }

    private static List<CategoryFilter> ExtractCategories(IReadOnlyDictionary<string, IAggregate> aggs)
    {
        var nested = GetNestedAggregation(aggs, ElasticsearchAggregationKeys.Categories);
        if (nested == null)
        {
            return [];
        }

        var ids = GetTermsBuckets(nested, ElasticsearchAggregationKeys.CategoryIds);
        var names = GetTermsBuckets(nested, ElasticsearchAggregationKeys.CategoryNames);

        var result = ids.Zip(names, (id, name) => new
        {
            Id = Guid.TryParse(id.Key.ToString(), out var guid) ? guid : (Guid?)null,
            Name = name.Key.ToString(),
            Count = (int)id.DocCount
        })
        .Where(x => x.Id.HasValue)
        .Select(x => new CategoryFilter(x.Id!.Value, x.Name, x.Count))
        .ToList();

        return result;
    }

    private static PriceRangeFilter ExtractPriceRange(IReadOnlyDictionary<string, IAggregate> aggs)
    {
        var nested = GetNestedAggregation(aggs, ElasticsearchAggregationKeys.PriceStats);
        if (nested == null)
        {
            return new PriceRangeFilter(0, 0, []);
        }

        var min = GetMetricValue<MinAggregate>(nested, ElasticsearchAggregationKeys.MinPrice);
        var max = GetMetricValue<MaxAggregate>(nested, ElasticsearchAggregationKeys.MaxPrice);

        return new PriceRangeFilter(min, max, []);
    }

    private static List<AttributeFilter> ExtractAttributes(IReadOnlyDictionary<string, IAggregate> aggs)
    {
        var variantNested = GetNestedAggregation(aggs, ElasticsearchAggregationKeys.VariantAttributes);
        if (variantNested == null)
        {
            return [];
        }

        var attrNested = GetNestedAggregation(variantNested, ElasticsearchAggregationKeys.Attributes);
        if (attrNested == null)
        {
            return [];
        }

        var nameBuckets = GetTermsBuckets(attrNested, ElasticsearchAggregationKeys.AttributeNames);

        return nameBuckets
            .Select(nameBucket =>
            {
                var name = nameBucket.Key.ToString();

                // Get the ID from the nested aggregation within this name bucket
                var idBuckets = nameBucket.Aggregations != null
                    ? GetTermsBuckets(nameBucket.Aggregations, ElasticsearchAggregationKeys.AttributeIds)
                    : [];

                var id = idBuckets.FirstOrDefault()?.Key.ToString();
                var parsedId = Guid.TryParse(id, out var guid) ? guid : (Guid?)null;

                if (!parsedId.HasValue)
                {
                    return null;
                }

                var values = ExtractAttributeValues(nameBucket);

                return new AttributeFilter(parsedId.Value, name, values);
            })
            .Where(af => af != null && af.Values.Count > 0)
            .ToList()!;
    }

    private static List<AttributeValueFilter> ExtractAttributeValues(StringTermsBucket bucket)
    {
        if (bucket.Aggregations == null) return [];

        var valueBuckets = GetTermsBuckets(
            bucket.Aggregations,
            ElasticsearchAggregationKeys.AttributeValues
        );

        return valueBuckets
            .Select(vb => new AttributeValueFilter(
                vb.Key.ToString(),
                (int)vb.DocCount,
                null
            ))
            .ToList();
    }

    private static IReadOnlyDictionary<string, IAggregate>? GetNestedAggregation(IReadOnlyDictionary<string, IAggregate> aggs, string key) =>
        aggs.TryGetValue(key, out var agg) && agg is NestedAggregate nested
            ? nested.Aggregations
            : null;

    private static IReadOnlyCollection<StringTermsBucket> GetTermsBuckets(IReadOnlyDictionary<string, IAggregate> aggs, string key) =>
        aggs.TryGetValue(key, out var agg) && agg is StringTermsAggregate terms
            ? terms.Buckets
            : [];

    private static double? GetMetricValue<T>(
        IReadOnlyDictionary<string, IAggregate> aggs,
        string key) where T : IAggregate
    {
        if (!aggs.TryGetValue(key, out var agg))
        {
            return null;
        }

        return agg switch
        {
            MinAggregate min => min.Value,
            MaxAggregate max => max.Value,
            _ => null
        };
    }
}