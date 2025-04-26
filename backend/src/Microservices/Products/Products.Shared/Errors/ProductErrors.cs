using Shared.BuildingBlocks.Result;

namespace Products.Shared.Errors;

public static class ProductErrors
{
    public static readonly ResultError NotFound = new("Product.NotFound", "Product not found.");
    public static readonly ResultError NegativeAmount = new("Product.Price.NegativeAmount", "The amount of a price can not be negative.");
}