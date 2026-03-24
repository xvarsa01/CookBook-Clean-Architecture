using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Specifications.Recipe;

public class RecipesByContainingIngredientId(Guid ingredientId) : ISpecification<RecipeBase, RecipeBase>
{
    
    public IQueryable<RecipeBase> UseFilter(IQueryable<RecipeBase> queryable)
    {
        queryable = queryable.Where(r => r.Ingredients.Any(i => i.IngredientId == ingredientId));
        return queryable;
    }
}
