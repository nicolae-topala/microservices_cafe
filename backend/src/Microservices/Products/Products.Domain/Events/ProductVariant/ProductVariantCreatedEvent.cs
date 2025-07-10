using Shared.Abstractions;

namespace Products.Domain.Events.ProductVariant;

public record ProductVariantCreatedEvent(Guid ProductId, Guid ProductVariantId) : IDomainEvent;
