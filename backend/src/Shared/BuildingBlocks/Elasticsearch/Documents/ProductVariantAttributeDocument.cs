namespace Shared.BuildingBlocks.Elasticsearch.Documents;

public record ProductVariantAttributeDocument(
    Guid Id,
    string Value,
    string AttributeName,
    string? UnitsOfMeasureName,
    string? UnitsOfMeasureAbbreviation);