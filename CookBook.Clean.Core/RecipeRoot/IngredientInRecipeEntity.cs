namespace CookBook.Clean.Core.RecipeRoot;

public record IngredientInRecipeEntity
{
    public Guid Id { get; init; }
    public Guid IngredientId { get; init; }

    public decimal Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    internal IngredientInRecipeEntity(Guid id, Guid ingredientId, decimal amount, MeasurementUnit unit)
    {
        Id = id;
        IngredientId = ingredientId;
        Amount = amount;
        Unit = unit;
    }

    public void Update(decimal newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
