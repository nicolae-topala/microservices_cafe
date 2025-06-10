using Inventory.Application.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Product;
using ItemDomain = Inventory.Domain.Entities.Item;

namespace Inventory.Application.Features.Product.Consumers;

public class ProductVariantCreatedConsumer(
    IInventoryDbContext dbContext,
    ILogger<ProductVariantCreatedConsumer> logger) 
    : IConsumer<ProductVariantCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductVariantCreatedEvent> context)
    {
        var message = context.Message;
        
        logger.LogInformation("Received ProductVariantCreatedEvent: ProductId={ProductId}, ProductVariantId={ProductVariantId}, IsInStock={IsInStock}", 
            message.ProductId, 
            message.ProductVariantId, 
            message.IsInStock);

        var itemResult = ItemDomain.Create(
            0,
            message.ProductVariantId,
            null,
            null);

        if (itemResult.IsFailure)
        {
            logger.LogError("Failed to create item: {Error}", itemResult.Error.Message);
        }

        dbContext.Items.Add(itemResult.Value);

        await dbContext.SaveChangesAsync();
    }
}
