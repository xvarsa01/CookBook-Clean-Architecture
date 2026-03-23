using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot;

public class IngredientInRecipeEntity
{
    public Guid Id { get; init; }
    public Guid IngredientId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    private IngredientInRecipeEntity() { } // for EF

    private IngredientInRecipeEntity(Guid id, Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        Id = id;
        IngredientId = ingredientId;
        Amount = amount;
        Unit = unit;
    }

    internal static Result<IngredientInRecipeEntity> Create(
        Guid id,
        Guid ingredientId,
        IngredientAmount amount,
        MeasurementUnit unit)
    {
        return Result.Ok(new IngredientInRecipeEntity(id, ingredientId, amount, unit));
    }


    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
