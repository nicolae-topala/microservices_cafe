using Shared.Abstractions;

namespace Products.Domain.Events.Product;

public record ProductCreatedEvent(Guid ProductId) : IDomainEvent;