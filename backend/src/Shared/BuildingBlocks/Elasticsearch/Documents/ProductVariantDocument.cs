namespace Shared.BuildingBlocks.Elasticsearch.Documents;

public record ProductVariantDocument(
    Guid Id, 
    bool IsInStock,
    bool IsVisible,
    decimal PriceAmount,
    string PriceCurrency,
    List<ProductVariantAttributeDocument> VariantAttributes);
