using Shared.Abstractions;

namespace Products.Domain.Events.Product;

public record ProductUpdatedEvent(Guid ProductId) : IDomainEvent;
