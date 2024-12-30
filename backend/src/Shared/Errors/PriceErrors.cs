using Shared.BuildingBlocks.Result;

namespace Shared.Errors;

public static class PriceErrors
{
    public static readonly ResultError NegativeAmount = new("Price.NegativeAmount", "The amount of a price can not be negative.");
}
