namespace CookBook.Clean.Core.RecipeRoot.Exceptions;

public sealed class RecipeHasNoIngredientsException : DomainException
{
    public RecipeHasNoIngredientsException(Guid recipeId) : base($"Recipe entity {recipeId} has no ingredients.")
    {
    }
}
