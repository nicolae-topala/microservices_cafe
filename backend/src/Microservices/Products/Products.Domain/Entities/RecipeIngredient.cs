using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Products.Domain.Entities;

public class RecipeIngredient : BaseEntity
{
    public decimal Quantity { get; private set; }
    public Guid UnitsOfMeasureId { get; private set; }
    public Guid RecipeId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public UnitsOfMeasure UnitsOfMeasure { get; private set; }
    public Recipe Recipe { get; private set; }
    public ProductVariant ProductVariant { get; private set; }

    private RecipeIngredient() { }

    private RecipeIngredient(
        Recipe recipe,
        ProductVariant productVariant,
        decimal quantity,
        UnitsOfMeasure unit)
    {
        Recipe = recipe;
        RecipeId = recipe.Id;
        ProductVariant = productVariant;
        ProductVariantId = productVariant.Id;
        Quantity = quantity;
        UnitsOfMeasure = unit;
        UnitsOfMeasureId = unit.Id;
    }

    public static Result<RecipeIngredient> Create(
        Recipe recipe,
        ProductVariant productVariant,
        decimal quantity,
        UnitsOfMeasure unit)
    {
        if (recipe is null)
        {
            return Result.Failure<RecipeIngredient>(new ResultError("Recipe.Invalid", "Recipe cannot be null"));
        }

        if (productVariant is null)
        {
            return Result.Failure<RecipeIngredient>(new ResultError("ProductVariant.Invalid", "Product variant cannot be null"));
        }

        if (unit is null)
        {
            return Result.Failure<RecipeIngredient>(new ResultError("Unit.Invalid", "Unit of measure cannot be null"));
        }

        if (quantity <= 0)
        {
            return Result.Failure<RecipeIngredient>(new ResultError("Quantity.Invalid", "Quantity must be greater than 0"));
        }

        return Result.Success(new RecipeIngredient(recipe, productVariant, quantity, unit));
    }

    public Result UpdateQuantity(decimal quantity)
    {
        if (quantity <= 0)
        {
            return Result.Failure(new ResultError("Quantity.Invalid", "Quantity must be greater than 0"));
        }

        Quantity = quantity;
        return Result.Success();
    }

    public Result UpdateUnit(UnitsOfMeasure unit)
    {
        if (unit is null)
        {
            return Result.Failure(new ResultError("Unit.Invalid", "Unit of measure cannot be null"));
        }

        UnitsOfMeasure = unit;
        return Result.Success();
    }

    public Result UpdateProductVariant(ProductVariant productVariant)
    {
        if (productVariant is null)
        {
            return Result.Failure(new ResultError("ProductVariant.Invalid", "Product variant cannot be null"));
        }

        ProductVariant = productVariant;
        return Result.Success();
    }

    public string GetFormattedQuantity()
    {
        return $"{Quantity} {UnitsOfMeasure.Abbreviation}";
    }

    public string GetIngredientDescription()
    {
        var description = $"{GetFormattedQuantity()} {ProductVariant.Product.Name}";

        if (ProductVariant.VariantAttributes.Count != 0)
        {
            description += $" - {ProductVariant.GetVariantDescription()}";
        }

        return description;
    }

    public Result<RecipeIngredient> Clone(Recipe newRecipe)
    {
        return Create(
            newRecipe,
            ProductVariant,
            Quantity,
            UnitsOfMeasure);
    }

    public Result Validate()
    {
        if (Recipe is null)
        {
            return Result.Failure(new ResultError("Recipe.Invalid", "Recipe cannot be null"));
        }

        if (ProductVariant is null)
        {
            return Result.Failure(new ResultError("ProductVariant.Invalid", "Product variant cannot be null"));
        }

        if (UnitsOfMeasure is null)
        {
            return Result.Failure(new ResultError("Unit.Invalid", "Unit of measure cannot be null"));
        }

        if (Quantity <= 0)
        {
            return Result.Failure(new ResultError("Quantity.Invalid", "Quantity must be greater than 0"));
        }

        return Result.Success();
    }

    public bool IsEquivalentTo(RecipeIngredient other)
    {
        if (other is null) return false;

        return ProductVariantId == other.ProductVariantId &&
               Quantity == other.Quantity &&
               UnitsOfMeasureId == other.UnitsOfMeasureId;
    }
}