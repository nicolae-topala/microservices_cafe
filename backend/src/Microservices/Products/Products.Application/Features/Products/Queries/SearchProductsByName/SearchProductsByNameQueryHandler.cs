using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Shared.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.Elasticsearch.Documents;

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
                .Bool(b =>
                {
                    b.Must(m => m
                        .Match(match => match
                            .Field(f => f.Name)
                            .Query(request.ProductName)
                            .Fuzziness(new Fuzziness(1))
                        )
                    )
                    .Filter(f => f
                        .Term(t => t.Field(x => x.IsVisible).Value(true))
                    );

                    if (request.VariantAttributes is not null && request.VariantAttributes.Count > 0)
                    {
                        foreach (var attribute in request.VariantAttributes)
                        {
                            b.Must(variantMust => variantMust
                                .Nested(n => n
                                    .Path(p => p.Variants)
                                    .Query(nestedQuery => nestedQuery
                                        .Bool(variantBool => variantBool
                                            .Must(attrMust => attrMust
                                                .Nested(nested => nested
                                                    .Path(p => p.Variants.First().VariantAttributes)
                                                    .Query(attrQuery => attrQuery
                                                        .Bool(attrBool => attrBool
                                                            .Must(
                                                                m => m.Match(match => match
                                                                    .Field(f => f.Variants.First().VariantAttributes.First().AttributeName)
                                                                    .Query(attribute.Key)),
                                                                m => m.Match(match => match
                                                                    .Field(f => f.Variants.First().VariantAttributes.First().Value)
                                                                    .Query(attribute.Value))
                                                            )
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
                })
            )
            .Sort(s => s
                .Score(x => x.Order(SortOrder.Desc))
            );

        if (request.Skip.HasValue)
            search.From(request.Skip.Value);

        if (request.Take.HasValue)
            search.Size(request.Take.Value);
    }
}
