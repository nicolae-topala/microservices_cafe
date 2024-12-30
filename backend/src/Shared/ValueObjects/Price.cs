using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Errors;
using Shared.Primitives;

namespace Shared.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Amount { get; private set; }
    public CurrencyEnum Currency { get; private set; }

    private Price(decimal amount, CurrencyEnum currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Price> Create(decimal amount, CurrencyEnum currency)
    {
        if (amount < 0m)
        {
            return Result.Failure<Price>(PriceErrors.NegativeAmount);
        }
        return Result.Create(new Price(amount, currency));
    }

    public Result Edit(decimal? amount, CurrencyEnum? currency)
    {
        if (amount.HasValue && amount < 0m)
            return Result.Failure<Price>(PriceErrors.NegativeAmount);

        if (amount.HasValue && amount != Amount)
            Amount = amount.Value;

        if (currency.HasValue && currency != Currency)
            Currency = currency.Value;

        return Result.Success();
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}