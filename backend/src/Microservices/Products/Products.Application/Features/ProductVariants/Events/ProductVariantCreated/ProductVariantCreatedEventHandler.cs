using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Application.Abstractions;
using Products.Domain.Events.ProductVariant;
using Shared.Abstractions;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.Elasticsearch.Documents;

namespace Products.Application.Features.ProductVariants.Events.ProductVariantCreated;

public class ProductVariantCreatedEventHandler(
    IProductsDbContext dbContext,
    IElasticsearchService elasticsearchService,
    IMapper mapper,
    ILogger<ProductVariantCreatedEventHandler> logger)
    : INotificationHandler<ProductVariantCreatedEvent>
{
    public async Task Handle(ProductVariantCreatedEvent notification, CancellationToken cancellationToken)
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
            logger.LogWarning("Product with ID {ProductId} not found for event {EventName}.",
                notification.ProductId, nameof(ProductVariantCreatedEvent));
            return;
        }

        var productDocument = mapper.Map<ProductDocument>(product);

        var success = await elasticsearchService.UpdateDocumentAsync(ElasticIndex.Products, productDocument, product.Id, cancellationToken);
        if (!success)
        {
            logger.LogError("Failed to update product {ProductId} in Elasticsearch after variant {VariantId} creation",
               product.Id, notification.ProductVariantId);
            return;
        }

        logger.LogInformation("Successfully updated product {ProductId} in Elasticsearch after variant {VariantId} creation",
            product.Id, notification.ProductVariantId);
    }
}
