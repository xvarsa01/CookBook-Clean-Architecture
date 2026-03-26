using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe;

public record IngredientInRecipe : EntityBase<IngredientInRecipeId>
{
    public IngredientId IngredientId { get; init; }
    public RecipeId RecipeId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    public Ingredient.Ingredient Ingredient { get => throw new InvalidOperationException();
        private set => throw new InvalidOperationException();
    }

    private IngredientInRecipe() { } // for EF

    private IngredientInRecipe(IngredientInRecipeId id,  IngredientId ingredientId, RecipeId recipeId, IngredientAmount amount, MeasurementUnit unit)
    {
        Id = id;
        IngredientId = ingredientId;
        RecipeId = recipeId;
        Amount = amount;
        Unit = unit;
    }

    internal static Result<IngredientInRecipe> Create(IngredientId ingredientId, RecipeId recipeId, IngredientAmount amount, MeasurementUnit unit) 
    {
        var id = new IngredientInRecipeId(Guid.NewGuid());
        return Result.Ok(new IngredientInRecipe(id, ingredientId, recipeId, amount, unit));
    }


    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
