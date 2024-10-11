using Shared.BuildingBlocks.Result;

namespace Shared.Errors;

public static class PriceErrors
{
    public static readonly Error NegativeAmount = new("Error.Price.NegativeAmount", "The amount of a price can not be negative.");
}
