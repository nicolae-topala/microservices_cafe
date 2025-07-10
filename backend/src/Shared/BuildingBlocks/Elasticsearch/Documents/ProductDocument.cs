using Shared.Enums;

namespace Shared.BuildingBlocks.Elasticsearch.Documents;

public record ProductDocument(
    Guid Id,
    string Name,
    string Description,
    bool IsVisible,
    bool IsInStock,
    ProductType Type,
    List<CategoryDocument> Categories,
    List<ProductVariantDocument> Variants);