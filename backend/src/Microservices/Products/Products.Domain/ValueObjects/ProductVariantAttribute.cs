using Products.Shared.Enums;
using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Errors;
using Shared.Primitives;

namespace Products.Domain.ValueObjects;

public sealed class ProductVariantAttribute : ValueObject
{
    public ProductVariantTypes Key { get; init; }
    public string Name { get; init; }
    public MeasurementType? Type { get; init; }
    public decimal? Value { get; init; }

    private ProductVariantAttribute(string name, MeasurementType? type, decimal? value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public static Result<ProductVariantAttribute> Create(
        string name,
        MeasurementType? type = null,
        decimal? value = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<ProductVariantAttribute>(SizeErrors.EmptyName);

        if (type.HasValue && !value.HasValue)
            return Result.Failure<ProductVariantAttribute>(SizeErrors.MissingMeasurement);

        return Result.Success(new ProductVariantAttribute(name, type, value));
    }


    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
        if (Type.HasValue)
        {
            yield return Type.Value;
            yield return Value!.Value;
        }
    }
}