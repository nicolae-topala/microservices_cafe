using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Shared.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.Elasticsearch.Documents;
using static Shared.BuildingBlocks.Elasticsearch.ElasticsearchConstants;

namespace Products.Application.Features.Products.Queries.SearchProductsByName;

public class SearchProductsByNameQueryHandler(IElasticsearchService elasticsearchService)
    : IQueryHandler<SearchProductsByNameQuery, IEnumerable<ProductDocument>>
{
    public async Task<IEnumerable<ProductDocument>> Handle(SearchProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var searchResponse = await elasticsearchService.SearchAsync<ProductDocument>(
            ElasticIndex.Products,
            search => ConfigureSearch(search, request),
            cancellationToken);

        return searchResponse.Documents;
    }

    private static void ConfigureSearch(
        SearchRequestDescriptor<ProductDocument> search,
        SearchProductsByNameQuery request)
    {
        search
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Bool(innerBool => innerBool
                            // Exact phrase match 
                            .Should(s => s
                                .MatchPhrase(phrase => phrase
                                    .Field(f => f.Name)
                                    .Query(request.ProductName)
                                    .Boost(Boost.ExactPhrase)
                                )
                            )
                            // Partial word matching
                            .Should(s => s
                                .Wildcard(wildcard => wildcard
                                    .Field(f => f.Name)
                                    .Value(request.ProductName)
                                    .CaseInsensitive(true)
                                    .Boost(Boost.PartialMatch)
                                )
                            )
                            // Standard text search
                            .Should(s => s
                                .Match(match => match
                                    .Field(f => f.Name)
                                    .Query(request.ProductName)
                                    .Operator(Operator.Or)
                                    .Boost(Boost.StandardMatch)
                                )
                            )
                            // Description search
                            .Should(s => s
                                .Match(match => match
                                    .Field(f => f.Description)
                                    .Query(request.ProductName)
                                    .Operator(Operator.Or)
                                    .Boost(Boost.DescriptionMatch)
                                )
                            )
                            // Fuzzy search for typos 
                            .Should(s => s
                                .Match(match => match
                                    .Field(f => f.Name)
                                    .Query(request.ProductName)
                                    .Fuzziness(new Fuzziness(2))
                                    .Boost(Boost.FuzzyMatch)
                                )
                            )
                            .MinimumShouldMatch(1)
                        )
                    )
                    .Filter(f => f
                        .Term(t => t.Field(x => x.IsVisible).Value(false))
                    )
                )
            )
            .Sort(s => s
                .Score(x => x.Order(SortOrder.Desc))
                .Field(f => f.Name.Suffix(FieldSuffixes.Keyword), new FieldSort { Order = SortOrder.Asc })
            );

        search.Size(request.Take);
        search.From(request.Skip);
    }
}
