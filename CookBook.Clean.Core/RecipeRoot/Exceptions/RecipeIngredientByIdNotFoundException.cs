namespace CookBook.Clean.Core.RecipeRoot.Exceptions;

public sealed class RecipeIngredientByIdNotFoundException : DomainException
{
    public RecipeIngredientByIdNotFoundException(Guid ingredientId, Guid recipeId)
        : base($"Ingredient {ingredientId} not found in recipe {recipeId}.")
    {
    }
}
