using Shared.Abstractions;

namespace Shared.Contracts.Product;

public class ProductVariantCreatedEvent : IIntegrationEvent
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
    public bool IsInStock { get; set; }
}
