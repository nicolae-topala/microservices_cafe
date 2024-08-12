using MicroservicesCafe.Shared.Enums;
using MicroservicesCafe.Shared.Primitives;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Product.Domain.Entities;
public sealed class Product : AggregateRoot
{
    private readonly List<string> _ingredients;

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Price Price { get; private set; }
    public ProductTypeEnum Type { get; private set; }
    public IReadOnlyCollection<string> Ingredients => _ingredients;
    public Guid CategoryId { get; private set; }

    private Product(Guid id) : base(id)
    {
    }

    public Product Create() =>
        new Product(Guid.NewGuid());
}
