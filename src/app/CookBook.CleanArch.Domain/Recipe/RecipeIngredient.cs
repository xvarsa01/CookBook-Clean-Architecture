using CookBook.CleanArch.Domain.Ingredient.ValueObjects;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe;

public record RecipeIngredient : EntityBase<RecipeIngredientId>
{
    public IngredientId IngredientId { get; init; }
    public RecipeId RecipeId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    public Ingredient.Ingredient Ingredient
    {
        get => throw new InvalidOperationException();
        private set => throw new InvalidOperationException();
    }

    private RecipeIngredient(RecipeIngredientId id,  IngredientId ingredientId, RecipeId recipeId, IngredientAmount amount, MeasurementUnit unit) : base(id)
    {
        IngredientId = ingredientId;
        RecipeId = recipeId;
        Amount = amount;
        Unit = unit;
    }

    internal static Result<RecipeIngredient> Create(IngredientId ingredientId, RecipeId recipeId, IngredientAmount amount, MeasurementUnit unit) 
    {
        var id = new RecipeIngredientId(Guid.NewGuid());
        return Result.Ok(new RecipeIngredient(id, ingredientId, recipeId, amount, unit));
    }


    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
