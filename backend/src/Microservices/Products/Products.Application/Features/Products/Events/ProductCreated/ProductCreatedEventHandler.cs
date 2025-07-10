using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Application.Abstractions;
using Products.Domain.Events.Product;
using Shared.Abstractions;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Features.Products.Events.ProductCreated;

public class ProductCreatedEventHandler(
    IProductsDbContext dbContext,
    IElasticsearchService elasticsearchService,
    IMapper mapper,
    ILogger<ProductCreatedEventHandler> logger) 
    : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Include(p => p.Categories)
            .Include(p => p.Variants)
                .ThenInclude(v => v.VariantAttributes)
                    .ThenInclude(va => va.AttributeDefinition)
            .Include(p => p.Variants)
                .ThenInclude(v => v.VariantAttributes)
                    .ThenInclude(va => va.UnitsOfMeasure)
            .FirstOrDefaultAsync(x => x.Id == notification.ProductId, cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product with ID {ProductId} not for event {EventName}.", 
                notification.ProductId, 
                nameof(ProductCreatedEvent));
            return;
        }

        var productDocument = mapper.Map<ProductDocument>(product);

        var success = await elasticsearchService.IndexDocumentAsync(ElasticIndex.Products, productDocument, product.Id, cancellationToken);
        if (!success)
        {
            logger.LogError("Failed to index product {ProductId} to Elasticsearch", product.Id);
            return;
        }

        logger.LogInformation("Successfully indexed product {ProductId} to Elasticsearch", product.Id);
    }
}
