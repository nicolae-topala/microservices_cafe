using MicroservicesCafe.Shared.Primitives;

namespace MicroservicesCafe.Product.Domain.Entities;

public sealed class Category : BaseEntity
{
    private readonly List<Product> _products;

    public string Name { get; private set; }
    public IReadOnlyCollection<Product> Products => _products;

    public Category(Guid id) : base(id)
    {
    }
}
