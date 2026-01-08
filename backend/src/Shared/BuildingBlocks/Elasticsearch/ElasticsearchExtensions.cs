using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.BuildingBlocks.Elasticsearch.Documents;
using static Shared.BuildingBlocks.Elasticsearch.ElasticsearchConstants;

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

                var createIndexResponse = await client.Indices.CreateAsync(indexName, descriptor =>
                {
                    switch (index)
                    {
                        case ElasticIndex.Products:
                            CreateProductMapping(descriptor);
                            break;
                        default:
                            throw new InvalidOperationException($"{indexName} mapping doesn't exist!");
                    }
                });

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

    private static void CreateProductMapping(CreateIndexRequestDescriptor descriptor) =>
           descriptor.Mappings(m => m
               .Properties<ProductDocument>(p => p
                   .Keyword(k => k.Id)
                   .Text(t => t.Name, td => td.Fields(f => f.Keyword(FieldSuffixes.Keyword)))
                   .Text(t => t.Description)
                   .Boolean(b => b.IsVisible)
                   .Boolean(b => b.IsInStock)
                   .Keyword(k => k.Type)

                   .Nested(nameof(ProductDocument.Categories), n => n
                        .Properties(cp => cp
                            .Keyword(nameof(CategoryDocument.Id))
                            .Text(nameof(CategoryDocument.Name), t => t.Fields(f => f.Keyword(FieldSuffixes.Keyword)))
                            .Keyword(nameof(CategoryDocument.ParentCategoryId))
                        )
                   )
                   .Nested(nameof(ProductDocument.Variants), n => n
                        .Properties(vp => vp
                            .Keyword(nameof(ProductVariantDocument.Id))
                            .Boolean(nameof(ProductVariantDocument.IsInStock))
                            .Boolean(nameof(ProductVariantDocument.IsVisible))
                            .DoubleNumber(nameof(ProductVariantDocument.PriceAmount))
                            .Keyword(nameof(ProductVariantDocument.PriceCurrency))

                            .Nested(nameof(ProductVariantDocument.VariantAttributes), va => va
                                .Properties(ap => ap
                                    .Keyword(nameof(ProductVariantAttributeDocument.Id))
                                    .Keyword(nameof(ProductVariantAttributeDocument.Value))
                                    .Text(nameof(ProductVariantAttributeDocument.AttributeName), t => t.Fields(f => f.Keyword(FieldSuffixes.Keyword)))
                                    .Text(nameof(ProductVariantAttributeDocument.UnitsOfMeasureName), t => t.Fields(f => f.Keyword(FieldSuffixes.Keyword)))
                                    .Keyword(nameof(ProductVariantAttributeDocument.UnitsOfMeasureAbbreviation))
                                )
                            )
                        )
                   )
               )
           );
}
