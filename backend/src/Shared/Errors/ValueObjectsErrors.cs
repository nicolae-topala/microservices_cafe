using Shared.BuildingBlocks.Result;

namespace Shared.Errors;

public static class ValueObjectsErrors
{
    public static readonly ResultError NegativeAmount = new("Price.NegativeAmount", "The amount of a price can not be negative.");
}

public static class SizeErrors
{
    public static readonly ResultError EmptyName = new("Size.EmptyName", "Size name cannot be empty");
    public static readonly ResultError EmptyDisplayValue = new("Size.EmptyDisplayValue", "Size display value cannot be empty");
    public static readonly ResultError InvalidPriceModifier = new("Size.InvalidPriceModifier", "Price modifier cannot be negative");
    public static readonly ResultError MissingMeasurement = new("Size.MissingMeasurement", "Measurement value is required when type is specified");
}