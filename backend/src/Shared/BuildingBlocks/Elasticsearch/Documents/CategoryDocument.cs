namespace Shared.BuildingBlocks.Elasticsearch.Documents;

public record CategoryDocument(
    Guid Id,
    string Name, 
    Guid? ParentCategoryId);