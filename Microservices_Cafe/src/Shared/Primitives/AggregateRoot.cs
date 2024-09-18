using Shared.Abstractions;

namespace Shared.Primitives;

public class AggregateRoot : BaseEntity
{
	private readonly List<IDomainEvent> _domainEvents = new();

	protected AggregateRoot(): base() { }

	protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
		_domainEvents.Add(domainEvent);

	public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents;

	public void ClearDomainEvents() => _domainEvents.Clear();
}
