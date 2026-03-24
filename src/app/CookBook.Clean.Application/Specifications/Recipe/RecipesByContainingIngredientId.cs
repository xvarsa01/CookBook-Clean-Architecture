using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Specifications.Recipe;

public class RecipesByContainingIngredientId(Guid ingredientId) : ISpecification<Core.RecipeRoot.Recipe, Core.RecipeRoot.Recipe>
{
    
    public IQueryable<Core.RecipeRoot.Recipe> UseFilter(IQueryable<Core.RecipeRoot.Recipe> queryable)
    {
        queryable = queryable.Where(r => r.Ingredients.Any(i => i.IngredientId == ingredientId));
        return queryable;
    }
}
