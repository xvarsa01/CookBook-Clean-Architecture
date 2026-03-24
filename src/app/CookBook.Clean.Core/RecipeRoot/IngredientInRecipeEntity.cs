using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.Core.RecipeRoot;

public record IngredientInRecipeEntity : EntityBase
{
    public override Guid Id { get; init; } = Guid.NewGuid();
    public Guid IngredientId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    private IngredientInRecipeEntity() { } // for EF

    private IngredientInRecipeEntity(Guid ingredientId, IngredientAmount amount, MeasurementUnit unit)
    {
        IngredientId = ingredientId;
        Amount = amount;
        Unit = unit;
    }

    internal static Result<IngredientInRecipeEntity> Create(Guid ingredientId,
        IngredientAmount amount,
        MeasurementUnit unit)
    {
        return Result.Ok(new IngredientInRecipeEntity(ingredientId, amount, unit));
    }


    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
}
