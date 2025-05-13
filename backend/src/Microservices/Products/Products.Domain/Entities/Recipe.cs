using Shared.BuildingBlocks.Result;
using Shared.Primitives;

namespace Products.Domain.Entities;

public class Recipe : AggregateRoot
{
    private readonly List<RecipeIngredient> _ingredients = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Instructions { get; private set; }
    public int PreparationTimeInMinutes { get; private set; }
    public bool IsPublished { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; }
    public IReadOnlyCollection<RecipeIngredient> Ingredients => _ingredients;

    private Recipe() { }

    private Recipe(
        string name,
        string description,
        ProductVariant productVariant,
        string instructions,
        int preparationTimeInMinutes)
    {
        Name = name;
        Description = description;
        ProductVariant = productVariant;
        ProductVariantId = productVariant.Id;
        Instructions = instructions;
        PreparationTimeInMinutes = preparationTimeInMinutes;
        IsPublished = false;
    }

    public static Result<Recipe> Create(
        string name,
        string description,
        ProductVariant productVariant,
        string instructions,
        int preparationTimeInMinutes)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Recipe>(new ResultError("Recipe.InvalidName", "Recipe name cannot be empty"));
        }

        if (productVariant is null)
        {
            return Result.Failure<Recipe>(new ResultError("Recipe.InvalidProductVariant", "Product variant cannot be null"));
        }

        if (string.IsNullOrWhiteSpace(instructions))
        {
            return Result.Failure<Recipe>(new ResultError("Recipe.InvalidInstructions", "Recipe instructions cannot be empty"));
        }

        if (preparationTimeInMinutes <= 0)
        {
            return Result.Failure<Recipe>(new ResultError("Recipe.InvalidPreparationTime", "Preparation time must be greater than 0"));
        }

        var recipe = new Recipe(
            name,
            description,
            productVariant,
            instructions,
            preparationTimeInMinutes);

        return Result.Success(recipe);
    }

    public Result AddIngredient(
        ProductVariant productVariant,
        decimal quantity,
        UnitsOfMeasure unit)
    {
        var ingredient = RecipeIngredient.Create(this, productVariant, quantity, unit);
        if (ingredient.IsFailure)
        {
            return Result.Failure(ingredient.Error);
        }

        _ingredients.Add(ingredient.Value);
        return Result.Success();
    }

    public Result UpdateIngredient(
        Guid ingredientId,
        decimal? quantity = null,
        UnitsOfMeasure? unit = null,
        ProductVariant? productVariant = null)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId);
        if (ingredient is null)
        {
            return Result.Failure(new ResultError("Recipe.IngredientNotFound", "Ingredient not found in recipe"));
        }

        if (quantity.HasValue)
        {
            var result = ingredient.UpdateQuantity(quantity.Value);
            if (result.IsFailure)
            {
                return result;
            }
        }

        if (unit is not null)
        {
            var result = ingredient.UpdateUnit(unit);
            if (result.IsFailure)
            {
                return result;
            }
        }

        if (productVariant is not null)
        {
            var result = ingredient.UpdateProductVariant(productVariant);
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Result.Success();
    }

    public Result RemoveIngredient(Guid ingredientId)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId);
        if (ingredient is null)
        {
            return Result.Failure(new ResultError("Recipe.IngredientNotFound", "Ingredient not found in recipe"));
        }

        _ingredients.Remove(ingredient);
        return Result.Success();
    }

    public Result ReorderIngredients(List<Guid> ingredientIdsInOrder)
    {
        // Validate that all IDs exist in the ingredients list
        foreach (var id in ingredientIdsInOrder)
        {
            if (!_ingredients.Any(i => i.Id == id))
            {
                return Result.Failure(new ResultError("Recipe.IngredientNotFound", $"Ingredient with ID {id} not found in recipe"));
            }
        }

        // Ensure all ingredients are included
        if (ingredientIdsInOrder.Count != _ingredients.Count)
        {
            return Result.Failure(new ResultError("Recipe.IncompleteSortOrder", "All ingredients must be included in the sort order"));
        }

        return Result.Success();
    }

    public Result UpdatePublishStatus(bool isPublished)
    {
        if (isPublished && !CanBePublished().IsSuccess)
        {
            return Result.Failure(new ResultError("Recipe.CannotPublish", "Recipe cannot be published. It may be missing required information or ingredients."));
        }

        IsPublished = isPublished;
        return Result.Success();
    }

    public Result CanBePublished()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return Result.Failure(new ResultError("Recipe.MissingName", "Recipe name is required"));
        }

        if (string.IsNullOrWhiteSpace(Instructions))
        {
            return Result.Failure(new ResultError("Recipe.MissingInstructions", "Recipe instructions are required"));
        }

        if (!_ingredients.Any())
        {
            return Result.Failure(new ResultError("Recipe.NoIngredients", "Recipe must have at least one ingredient"));
        }

        if (PreparationTimeInMinutes <= 0)
        {
            return Result.Failure(new ResultError("Recipe.InvalidPreparationTime", "Preparation time must be greater than 0"));
        }

        return Result.Success();
    }

    public Result UpdateRecipe(
        string? name = null,
        string? description = null,
        string? instructions = null,
        int? preparationTimeInMinutes = null,
        ProductVariant? productVariant = null)
    {
        if (name is not null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure(new ResultError("Recipe.InvalidName", "Recipe name cannot be empty"));
            }

            Name = name;
        }

        if (description is not null)
        {
            Description = description;
        }

        if (instructions is not null)
        {
            if (string.IsNullOrWhiteSpace(instructions))
            {
                return Result.Failure(new ResultError("Recipe.InvalidInstructions", "Recipe instructions cannot be empty"));
            }

            Instructions = instructions;
        }

        if (preparationTimeInMinutes.HasValue)
        {
            if (preparationTimeInMinutes.Value <= 0)
            {
                return Result.Failure(new ResultError("Recipe.InvalidPreparationTime", "Preparation time must be greater than 0"));
            }

            PreparationTimeInMinutes = preparationTimeInMinutes.Value;
        }

        if (productVariant is not null)
        {
            ProductVariant = productVariant;
            ProductVariantId = productVariant.Id;
        }

        return Result.Success();
    }

    public Result<Recipe> Clone()
    {
        var clonedRecipe = Create(
            $"{Name} (Copy)",
            Description,
            ProductVariant,
            Instructions,
            PreparationTimeInMinutes);

        if (clonedRecipe.IsFailure)
        {
            return Result.Failure<Recipe>(clonedRecipe.Error);
        }

        // Clone ingredients
        foreach (var ingredient in _ingredients)
        {
            var clonedIngredient = ingredient.Clone(clonedRecipe.Value);
            if (clonedIngredient.IsSuccess)
            {
                clonedRecipe.Value._ingredients.Add(clonedIngredient.Value);
            }
        }

        return clonedRecipe;
    }
}
