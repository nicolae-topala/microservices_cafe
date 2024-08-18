using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Shared.Errors;

public static class PriceErrors
{
    public static readonly Error NegativeAmmount = new("Error.Price.NegativeAmmount", "The ammount of a price can not be negative.");
}
