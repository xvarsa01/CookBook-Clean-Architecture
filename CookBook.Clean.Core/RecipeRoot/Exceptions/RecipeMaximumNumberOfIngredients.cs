namespace CookBook.Clean.Core.RecipeRoot.Exceptions;

public sealed class RecipeMaximumNumberOfIngredients : DomainException
{
    public RecipeMaximumNumberOfIngredients() : base("Recipe cannot have more than 10 ingredients.")
    {
    }
}
