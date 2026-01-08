using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.Fluent;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Products.Application.Abstractions;
using Shared.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.Elasticsearch.Documents;
using static Shared.BuildingBlocks.Elasticsearch.ElasticsearchConstants;

namespace Products.Application.Features.Products.Queries.SearchProductsWithFilters;

public class SearchProductsWithFiltersQueryHandler(
    IElasticsearchService elasticsearchService,
    IElasticsearchAggregationMapper aggregationMapper)
    : IQueryHandler<SearchProductsWithFiltersQuery, SearchProductsWithFiltersResponse>
{
    public async Task<SearchProductsWithFiltersResponse> Handle(
        SearchProductsWithFiltersQuery request,
        CancellationToken cancellationToken)
    {
        var searchResponse = await elasticsearchService.SearchAsync<ProductDocument>(
            ElasticIndex.Products,
            search => ConfigureSearch(search, request),
            cancellationToken);

        var products = searchResponse.Documents;
        var totalCount = searchResponse.Total;
        var filters = aggregationMapper.MapToSearchFilters(searchResponse.Aggregations);

        return new SearchProductsWithFiltersResponse(products, totalCount, filters);
    }

    private static void ConfigureSearch(
        SearchRequestDescriptor<ProductDocument> search,
        SearchProductsWithFiltersQuery request)
    {
        var useRandomScore = ShouldUseRandomScore(request);

        if (useRandomScore)
        {
            search.Query(q => q
                .FunctionScore(fs => fs
                    .Query(innerQ => BuildQuery(innerQ, request))
                    .Functions(f => f
                        .RandomScore(rs => rs
                            .Seed(DateTime.UtcNow.Date.GetHashCode())
                            .Field(f => f.Id)
                        )
                    )
                    .BoostMode(FunctionBoostMode.Replace)
                )
            );
        }
        else
        {
            search.Query(q => BuildQuery(q, request));
        }

        search
            .Sort(s => BuildSorting(s, request))
            .Size(request.Take)
            .From(request.Skip);

        var includeAggregations = request.Skip == 0;
        if (includeAggregations)
        {
            search.Aggregations(aggs =>
            {
                BuildAggregations(aggs);
                return aggs;
            });
        }
    }

    private static bool ShouldUseRandomScore(SearchProductsWithFiltersQuery request) =>
        string.IsNullOrWhiteSpace(request.ProductName);

    private static void BuildQuery(QueryDescriptor<ProductDocument> q, SearchProductsWithFiltersQuery request)
    {
        q.Bool(b =>
        {
            if (!string.IsNullOrWhiteSpace(request.ProductName))
            {
                b.Must(m => m
                    .Bool(textBool => textBool
                        .Should(
                            // Exact phrase match
                            s => s.MatchPhrase(phrase => phrase
                                .Field(f => f.Name)
                                .Query(request.ProductName)
                                .Boost(Boost.ExactPhrase)),

                            // Partial word matching
                            s => s.Wildcard(wildcard => wildcard
                                .Field(f => f.Name)
                                .Value(request.ProductName)
                                .CaseInsensitive(true)
                                .Boost(Boost.PartialMatch)),

                            // Standard text search
                            s => s.Match(match => match
                                .Field(f => f.Name)
                                .Query(request.ProductName)
                                .Operator(Operator.Or)
                                .Boost(Boost.StandardMatch)),

                            // Description search
                            s => s.Match(match => match
                                .Field(f => f.Description)
                                .Query(request.ProductName)
                                .Operator(Operator.Or)
                                .Boost(Boost.DescriptionMatch)),

                            // Fuzzy search for typos
                            s => s.Match(match => match
                                .Field(f => f.Name)
                                .Query(request.ProductName)
                                .Fuzziness(new Fuzziness(1))
                                .Boost(Boost.FuzzyMatch))
                        )
                        .MinimumShouldMatch(1)
                    ));
            }

            b.Filter(f => f
                .Term(t => t.Field(x => x.IsVisible).Value(false))
            );

            if (request.InStockOnly == true)
            {
                b.Filter(f => f
                    .Term(t => t.Field(x => x.IsInStock).Value(true))
                );
            }

            if (request.CategoryIds?.Count > 0)
            {
                foreach (var categoryId in request.CategoryIds)
                {
                    b.Filter(f => f
                        .Nested(n => n
                            .Path(p => p.Categories)
                            .Query(nestedQuery => nestedQuery
                                .Term(t => t.Field(f => f.Categories.First().Id).Value(categoryId.ToString()))
                            )
                        )
                    );
                }
            }

            if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
            {
                b.Filter(f => f
                    .Nested(n => n
                        .Path(p => p.Variants)
                        .Query(nestedQuery => nestedQuery
                            .Range(r => r
                                .NumberRange(nr => nr
                                    .Field(f => f.Variants.FirstOrDefault().PriceAmount)
                                    .Gte(request.MinPrice)
                                    .Lte(request.MaxPrice)
                                )
                            )
                        )
                    )
                );
            }

            if (request.VariantAttributesIds?.Count > 0)
            {
                foreach (var attributeId in request.VariantAttributesIds)
                {
                    b.Filter(f => f
                        .Nested(n => n
                            .Path(p => p.Variants)
                            .Query(variantQuery => variantQuery
                                .Nested(attrNested => attrNested
                                    .Path(p => p.Variants.FirstOrDefault().VariantAttributes)
                                    .Query(attrQuery => attrQuery
                                        .Bool(attrBool => attrBool
                                            .Must(
                                                m => m.Match(match => match
                                                    .Field(f => f.Variants.FirstOrDefault().VariantAttributes.FirstOrDefault().Id)
                                                    .Query(attributeId))
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    );
                }
            }
        });
    }

    private static void BuildSorting(
        SortOptionsDescriptor<ProductDocument> s,
        SearchProductsWithFiltersQuery request)
    {
        switch (request.SortBy)
        {
            case SortBy.Name:
                s.Field(f => f.Name.Suffix(FieldSuffixes.Keyword),
                    new FieldSort { Order = request.SortOrder });
                break;

            case SortBy.Price:
                s.Field(f => f.Variants.FirstOrDefault().PriceAmount,
                    new FieldSort { Order = request.SortOrder });
                break;

            case SortBy.Popularity:
                s.Score(score => score.Order(SortOrder.Desc));
                break;

            default:
                if (!string.IsNullOrWhiteSpace(request.ProductName))
                {
                    s.Score(score => score.Order(SortOrder.Desc))
                     .Field(f => f.Name.Suffix(FieldSuffixes.Keyword),
                         new FieldSort { Order = SortOrder.Asc });
                }
                else
                {
                    s.Score(score => score.Order(SortOrder.Desc));
                }
                break;
        }
    }

    private static void BuildAggregations(
        FluentDescriptorDictionary<string, AggregationDescriptor<ProductDocument>> aggs)
    {
        BuildCategoryAggregation(aggs);
        BuildPriceRangeAggregation(aggs);
        BuildVariantAttributesAggregation(aggs);
    }

    private static void BuildCategoryAggregation(
        FluentDescriptorDictionary<string, AggregationDescriptor<ProductDocument>> aggs)
    {
        aggs.Add(ElasticsearchAggregationKeys.Categories, a => a
            .Nested(n => n.Path(p => p.Categories))
            .Aggregations(catAggs => catAggs
                .Add(ElasticsearchAggregationKeys.CategoryIds, t => t
                    .Terms(terms => terms
                        .Field(Infer.Field<ProductDocument>(f => f.Categories.First().Id))
                        .Size(50)
                    )
                )
                .Add(ElasticsearchAggregationKeys.CategoryNames, t => t
                    .Terms(terms => terms
                        .Field(Infer.Field<ProductDocument>(f => f.Categories.First().Name.Suffix(FieldSuffixes.Keyword)))
                        .Size(50)
                    )
                )
            )
        );
    }

    private static void BuildPriceRangeAggregation(
        FluentDescriptorDictionary<string, AggregationDescriptor<ProductDocument>> aggs)
    {
        aggs.Add(ElasticsearchAggregationKeys.PriceStats, a => a
            .Nested(n => n.Path(p => p.Variants))
            .Aggregations(priceAggs => priceAggs
                .Add(ElasticsearchAggregationKeys.MinPrice, m => m
                    .Min(min => min.Field(f => f.Variants.First().PriceAmount))
                )
                .Add(ElasticsearchAggregationKeys.MaxPrice, m => m
                    .Max(max => max.Field(f => f.Variants.First().PriceAmount))
                )
            )
        );
    }

    private static void BuildVariantAttributesAggregation(
        FluentDescriptorDictionary<string, AggregationDescriptor<ProductDocument>> aggs)
    {
        aggs.Add(ElasticsearchAggregationKeys.VariantAttributes, a => a
            .Nested(n => n.Path(p => p.Variants))
            .Aggregations(variantAggs => variantAggs
                .Add(ElasticsearchAggregationKeys.Attributes, na => na
                    .Nested(nested => nested.Path(p => p.Variants.FirstOrDefault().VariantAttributes))
                    .Aggregations(attrAggs => attrAggs
                        .Add(ElasticsearchAggregationKeys.AttributeNames, t => t
                            .Terms(terms => terms
                                .Field(Infer.Field<ProductDocument>(f => f.Variants.FirstOrDefault().VariantAttributes.FirstOrDefault()
                                    .AttributeName.Suffix(FieldSuffixes.Keyword)))
                                .Size(50)
                            )
                            .Aggregations(nameAggs => nameAggs
                                .Add(ElasticsearchAggregationKeys.AttributeIds, id => id
                                    .Terms(idTerms => idTerms
                                        .Field(Infer.Field<ProductDocument>(f => f.Variants.FirstOrDefault().VariantAttributes.FirstOrDefault().Id))
                                        .Size(1)
                                    )
                                )
                                .Add(ElasticsearchAggregationKeys.AttributeValues, vt => vt
                                    .Terms(vterms => vterms
                                        .Field(Infer.Field<ProductDocument>(f => f.Variants.FirstOrDefault().VariantAttributes.FirstOrDefault().Value))
                                        .Size(100)
                                    )
                                )
                            )
                        )
                    )
                )
            )
        );
    }
}