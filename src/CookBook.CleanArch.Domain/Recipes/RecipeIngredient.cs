using System.Diagnostics;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipes;

public record RecipeIngredient : EntityBase<RecipeIngredientId>
{
    public IngredientId IngredientId { get; init; }
    public RecipeId RecipeId { get; init; }

    public IngredientAmount Amount { get; private set; }
    public MeasurementUnit Unit { get; private set; }

    private Ingredient? _ingredient;
    public Ingredient Ingredient
    {
        get => GetIngredientForReadModelOnly();
        private set => _ingredient = value;     // only for EF fixup/materialization
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
        return Result.Success(new RecipeIngredient(id, ingredientId, recipeId, amount, unit));
    }


    public void Update(IngredientAmount newAmount, MeasurementUnit newUnit)
    {
        Amount = newAmount;
        Unit = newUnit;
    }
    
    /// <summary>
    /// EF navigation used for query translation only.
    /// Accessing it in domain/business runtime code is invalid because it crosses aggregate boundaries.
    /// In production this throws to fail fast; in debug it logs and returns null-forgiving value
    /// to avoid blocking local debug/seeding flows.
    /// </summary>
    private Ingredient GetIngredientForReadModelOnly()
    {
        const string message =
            "RecipeIngredient.Ingredient is an EF read-model navigation only. " +
            "Use IngredientId in domain logic; load Ingredient explicitly in application queries.";

        Trace.TraceWarning(message);

#if DEBUG
        return _ingredient ?? null!;
#else
        throw new InvalidOperationException(message);
#endif
    }
}
