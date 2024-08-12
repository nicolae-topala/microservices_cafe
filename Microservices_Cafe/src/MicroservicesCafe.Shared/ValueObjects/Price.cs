using MicroservicesCafe.Shared.BuildingBlocks.Result;
using MicroservicesCafe.Shared.Errors;
using MicroservicesCafe.Shared.Primitives;

namespace MicroservicesCafe.Shared.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Value { get; private set; }
    public CurrencyEnum Currency { get; private set; }

    private Price(decimal value, CurrencyEnum currency)
    {
        Value = value;
        Currency = currency;
    }

    public static Result<Price> Create(decimal value, CurrencyEnum currency)
    {
        if (value < 0)
        {
            return Result.Failure<Price>(PriceErrors.NegativeValue);
        }

        return new Price(value, currency);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return Currency;
    }
}

public enum CurrencyEnum
{
    EUR,
    USD,
}