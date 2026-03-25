using CookBook.CleanArch.Domain.RecipeRoot.Enums;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.RecipeRoot;

public record IngredientInRecipeEntity : EntityBase<IngredientInRecipeId>
{
    public Guid IngredientId { get; init; }
    public Guid RecipeId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    private IngredientInRecipeEntity() { } // for EF

    private IngredientInRecipeEntity(Guid ingredientId, Guid recipeId, IngredientAmount amount, MeasurementUnit unit)
    {
        Id = new IngredientInRecipeId(Guid.NewGuid());
        IngredientId = ingredientId;
        RecipeId = recipeId;
        Amount = amount;
        Unit = unit;
    }

    internal static Result<IngredientInRecipeEntity> Create(Guid ingredientId, Guid recipeId, IngredientAmount amount, MeasurementUnit unit) 
    {
        return Result.Ok(new IngredientInRecipeEntity(ingredientId, recipeId, amount, unit));
    }


    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
