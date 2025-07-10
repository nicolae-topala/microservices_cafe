using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Features.Products.Queries.SearchProductsByName;

public record SearchProductsByNameQuery(
    string ProductName,
    int? Skip = null,
    int? Take = null,

    // Category filters
    List<Guid>? CategoryIds = null,

    // Price range filters
    decimal? MinPrice = null,
    decimal? MaxPrice = null,

    // Stock filter
    bool? InStockOnly = null,

    // Variant attribute filters
    Dictionary<string, string>? VariantAttributes = null)
    : IQuery<IEnumerable<ProductDocument>>;