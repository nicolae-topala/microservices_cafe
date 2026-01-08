using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Products.Domain.Entities;

public sealed class ProductVariantAttribute : BaseEntity
{
    public string Value { get; private set; }
    public Guid AttributeDefinitionId { get; private set; }
    public Guid? UnitsOfMeasureId { get; private set; }  
    public Guid ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; }
    public VariantAttributeDefinition AttributeDefinition { get; private set; }
    public UnitsOfMeasure? UnitsOfMeasure { get; private set; }

    private ProductVariantAttribute() { }

    private ProductVariantAttribute(
            ProductVariant productVariant,
            VariantAttributeDefinition attributeDefinition,
            string value,
            UnitsOfMeasure? unitsOfMeasure)
    {
        AttributeDefinition = attributeDefinition;
        AttributeDefinitionId = attributeDefinition.Id;
        Value = value;
        UnitsOfMeasure = unitsOfMeasure;
        UnitsOfMeasureId = unitsOfMeasure?.Id;
        ProductVariant = productVariant;
        ProductVariantId = productVariant.Id;
    }

    public static Result<ProductVariantAttribute> Create(
        ProductVariant productVariant,
        VariantAttributeDefinition attributeDefinition,
        string value,
        UnitsOfMeasure? unitsOfMeasure = null)
    {
        if (attributeDefinition is null)
        {
            return Result.Failure<ProductVariantAttribute>(new ResultError("Attribute.NullDefinition", "Attribute definition cannot be null."));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<ProductVariantAttribute>(new ResultError("Attribute.EmptyValue", "Attribute value cannot be empty."));
        }

        return Result.Success(new ProductVariantAttribute(productVariant, attributeDefinition, value, unitsOfMeasure));
    }

    public Result UpdateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure(new ResultError("Attribute.EmptyValue", "Attribute value cannot be empty."));
        }

        Value = value;
        return Result.Success();
    }

    public Result UpdateUnitsOfMeasure(UnitsOfMeasure? unitsOfMeasure)
    {
        UnitsOfMeasure = unitsOfMeasure;
        UnitsOfMeasureId = unitsOfMeasure?.Id; 
        return Result.Success();
    }

    public Result UpdateAttributeDefinition(VariantAttributeDefinition attributeDefinition)
    {
        if (attributeDefinition is null)
        {
            return Result.Failure(new ResultError("Attribute.NullDefinition", "Attribute definition cannot be null."));
        }

        AttributeDefinition = attributeDefinition;
        AttributeDefinitionId = attributeDefinition.Id;
        return Result.Success();
    }

    public string GetFormattedValue()
    {
        return UnitsOfMeasure != null
            ? $"{Value} {UnitsOfMeasure.Abbreviation}"
            : Value;
    }

    public string GetAttributeDescription()
    {
        return $"{AttributeDefinition.Name}: {GetFormattedValue()}";
    }

    public bool HasSameDefinition(ProductVariantAttribute other)
    {
        return AttributeDefinitionId == other.AttributeDefinitionId;
    }

    public Result Validate()
    {
        if (AttributeDefinition is null)
        {
            return Result.Failure(new ResultError("Attribute.NullDefinition", "Attribute definition cannot be null."));
        }

        if (string.IsNullOrWhiteSpace(Value))
        {
            return Result.Failure(new ResultError("Attribute.EmptyValue", "Attribute value cannot be empty."));
        }

        return Result.Success();
    }

    public Result<ProductVariantAttribute> Clone()
    {
        return Create(
            ProductVariant,
            AttributeDefinition,
            Value,
            UnitsOfMeasure);
    }

    public bool IsEquivalentTo(ProductVariantAttribute other)
    {
        if (other is null) return false;

        return AttributeDefinitionId == other.AttributeDefinitionId &&
               Value == other.Value &&
               UnitsOfMeasureId == other.UnitsOfMeasureId;
    }
}