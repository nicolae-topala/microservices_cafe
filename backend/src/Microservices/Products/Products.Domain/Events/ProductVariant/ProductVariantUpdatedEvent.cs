using Shared.Abstractions;

namespace Products.Domain.Events.ProductVariant;

public record ProductVariantUpdatedEvent(Guid ProductId, Guid ProductVariantId) : IDomainEvent;
