﻿using Shared.BuildingBlocks.Result;
using Shared.Enums;
using Shared.Errors;
using Shared.Primitives;

namespace Shared.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Ammount { get; private set; }
    public CurrencyEnum Currency { get; private set; }

    private Price(decimal ammount, CurrencyEnum currency)
    {
        Ammount = ammount;
        Currency = currency;
    }

    public static Result<Price> Create(decimal ammount, CurrencyEnum currency)
    {
        if (ammount < 0)
        {
            return Result.Failure<Price>(PriceErrors.NegativeAmmount);
        }

        return new Price(ammount, currency);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Ammount;
        yield return Currency;
    }
}