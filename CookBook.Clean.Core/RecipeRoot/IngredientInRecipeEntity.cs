using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Core.RecipeRoot;

public record IngredientInRecipeEntity
{
    public Guid Id { get; init; }
    public Guid IngredientId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    internal IngredientInRecipeEntity(Guid id, Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        Id = id;
        IngredientId = ingredientId;
        Amount = amount;
        Unit = unit;
    }

    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
