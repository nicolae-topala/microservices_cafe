using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Errors;
using Shared.Primitives;

namespace Shared.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }

    private Price(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Price> Create(decimal amount, Currency currency)
    {
        if (amount < 0m)
        {
            return Result.Failure<Price>(ValueObjectsErrors.NegativeAmount);
        }
        return Result.Create(new Price(amount, currency));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}