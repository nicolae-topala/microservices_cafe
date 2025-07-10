using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Shared.Abstractions;

namespace Shared.BuildingBlocks.Elasticsearch;

public class ElasticsearchService(ElasticsearchClient client, ILogger<ElasticsearchService> logger)
    : IElasticsearchService
{
    public async Task<bool> IndexDocumentAsync<T>(ElasticIndex index, T document, Guid documentId, CancellationToken cancellationToken)
    {
        var indexName = index.ToString().ToLowerInvariant();
        var response = await client.IndexAsync(document, documentId ,idx => idx.Index(indexName), cancellationToken);
        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to index document: {Error}", response.DebugInformation);
        }
        return response.IsValidResponse;
    }

    public async Task<bool> UpdateDocumentAsync<T>(ElasticIndex index, T document, Guid documentId, CancellationToken cancellationToken)
    {
        var indexName = index.ToString().ToLowerInvariant();
        var response = await client.UpdateAsync<T, T>(indexName, documentId, u => u.Doc(document), cancellationToken);
        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to update document: {Error}", response.DebugInformation);
        }
        return response.IsValidResponse;
    }

    public async Task<bool> DeleteDocumentAsync(ElasticIndex index, Guid id, CancellationToken cancellationToken)
    {
        var indexName = index.ToString().ToLowerInvariant();
        var response = await client.DeleteAsync(indexName, id, cancellationToken);
        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to delete document: {Error}", response.DebugInformation);
        }
        return response.IsValidResponse;
    }

    public async Task<T?> GetDocumentAsync<T>(ElasticIndex index, Guid id, CancellationToken cancellationToken) 
        where T : class
    {
        var indexName = index.ToString().ToLowerInvariant();
        var response = await client.GetAsync<T>(indexName, id, cancellationToken);
        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to get document: {Error}", response.DebugInformation);
            return null;
        }
        return response.Source;
    }

    public async Task<SearchResponse<T>> SearchAsync<T>(ElasticIndex index, Action<SearchRequestDescriptor<T>> searchDescriptor, CancellationToken cancellationToken)
           where T : class
    {
        var indexName = index.ToString().ToLowerInvariant();
        var response = await client.SearchAsync(indexName, searchDescriptor, cancellationToken);
        if (!response.IsValidResponse)
        {
            logger.LogError("Search failed: {Error}", response.DebugInformation);
            throw new InvalidOperationException($"Search operation failed: {response.DebugInformation}");
        }
        return response;
    }
}