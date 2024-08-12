using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Shared.Errors;

public static class PriceErrors
{
    public static readonly Error NegativeValue = new("Error.Price.NegativeValue", "The value of a price can not be negative.");
}
