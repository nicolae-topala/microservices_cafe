using Elastic.Clients.Elasticsearch;
using Shared.BuildingBlocks.Elasticsearch;

namespace Shared.Abstractions;

public interface IElasticsearchService
{
    Task<bool> IndexDocumentAsync<T>(ElasticIndex index, T document, Guid documentId, CancellationToken cancellationToken);
    Task<bool> UpdateDocumentAsync<T>(ElasticIndex index, T document, Guid documentId, CancellationToken cancellationToken);
    Task<bool> DeleteDocumentAsync(ElasticIndex index, Guid id, CancellationToken cancellationToken);
    Task<T?> GetDocumentAsync<T>(ElasticIndex index, Guid id, CancellationToken cancellationToken) where T : class;
    Task<SearchResponse<T>> SearchAsync<T>(ElasticIndex index, Action<SearchRequestDescriptor<T>> searchDescriptor, CancellationToken cancellationToken) where T : class;
}