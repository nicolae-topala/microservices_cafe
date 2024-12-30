using Shared.BuildingBlocks.Result;

namespace Products.Shared.Errors;

public static class ProductErrors
{
    public static readonly ResultError NegativeAmount = new("Price.NegativeAmount", "The amount of a price can not be negative.");
}