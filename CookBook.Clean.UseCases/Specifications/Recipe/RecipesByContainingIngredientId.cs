using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.UseCases.Specifications.Recipe;

public class RecipesByContainingIngredientId(Guid ingredientId) : ISpecification<RecipeEntity, RecipeEntity>
{
    
    public IQueryable<RecipeEntity> UseFilter(IQueryable<RecipeEntity> queryable)
    {
        queryable = queryable.Where(r => r.Ingredients.Any(i => i.IngredientId == ingredientId));
        return queryable;
    }
}
