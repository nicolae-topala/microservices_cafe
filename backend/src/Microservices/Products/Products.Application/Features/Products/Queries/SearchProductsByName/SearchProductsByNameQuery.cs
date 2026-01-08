using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Features.Products.Queries.SearchProductsByName;

public record SearchProductsByNameQuery(
    string ProductName,
    int Skip,
    int Take)
    : IQuery<IEnumerable<ProductDocument>>;