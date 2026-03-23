namespace CookBook.Clean.Core.RecipeRoot.Exceptions;

public sealed class RecipeIngredientByEntryIdNotFoundException : DomainException
{
    public RecipeIngredientByEntryIdNotFoundException(Guid ingredientEntryId, Guid recipeId)
        : base($"Ingredient entry for {ingredientEntryId} not found in recipe {recipeId}.")
    {
    }
}
