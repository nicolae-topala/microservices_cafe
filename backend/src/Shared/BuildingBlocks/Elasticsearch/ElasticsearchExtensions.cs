using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Shared.BuildingBlocks.Elasticsearch;

public static class ElasticsearchExtensions
{
    public static async Task InitializeElasticsearchIndicesAsync(
        this IApplicationBuilder app,
        params ElasticIndex[] indices)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ElasticsearchClient>>();

        foreach (var index in indices)
        {
            var indexName = index.ToString().ToLowerInvariant();

            try
            {
                var indexExists = await client.Indices.ExistsAsync(indexName);
                if (indexExists.Exists)
                {
                    logger.LogInformation("{IndexName} index already exists, skipping creation", indexName);
                    continue;
                }

                var createIndexResponse = await client.Indices.CreateAsync(indexName);

                if (createIndexResponse.IsValidResponse)
                {
                    logger.LogInformation("Successfully created {IndexName} index", indexName);
                }
                else
                {
                    logger.LogError("Failed to create {IndexName} index: {Error}",
                        indexName, createIndexResponse.DebugInformation);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating {IndexName} index", indexName);
            }
        }
    }

    private static async Task<CreateIndexResponse> CreateProductsIndexAsync(
        ElasticsearchClient client,
        string indexName)
    {
        return await client.Indices.CreateAsync(indexName, c => c
            .Mappings(m => m
                .Properties<ProductDocument>(p => p
                    .Keyword(k => k.Id)
                    .Text(t => t.Name)
                    .Text(t => t.Description)
                    .Boolean(b => b.IsVisible)
                    .Boolean(b => b.IsInStock)
                    .Keyword(k => k.Type)
                    .Nested(p => p.Categories)
                    .Nested(n => n.Variants)
                    )
                )
        );
    }
}
