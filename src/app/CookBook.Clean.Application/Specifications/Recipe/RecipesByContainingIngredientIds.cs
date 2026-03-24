using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Specifications.Recipe;

public class RecipesByContainingIngredientIds(List<Guid> ids) : ISpecification<RecipeBase, RecipeBase>
{
    public IQueryable<RecipeBase> UseFilter(IQueryable<RecipeBase> queryable)
    {
        queryable =  queryable.Where(r => r.Ingredients.Any(i => ids.Contains(i.IngredientId)));
        return queryable;
    }
}
