namespace CookBook.Clean.Core.Recipe;

public record IngredientInRecipeEntity(Guid Id, Guid IngredientId, decimal Amount, MeasurementUnit Unit);
