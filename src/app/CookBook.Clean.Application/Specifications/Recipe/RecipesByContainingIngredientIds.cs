using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Specifications.Recipe;

public class RecipesByContainingIngredientIds(List<Guid> ids) : ISpecification<Core.RecipeRoot.Recipe, Core.RecipeRoot.Recipe>
{
    public IQueryable<Core.RecipeRoot.Recipe> UseFilter(IQueryable<Core.RecipeRoot.Recipe> queryable)
    {
        queryable =  queryable.Where(r => r.Ingredients.Any(i => ids.Contains(i.IngredientId)));
        return queryable;
    }
}
