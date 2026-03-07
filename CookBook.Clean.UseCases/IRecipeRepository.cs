using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.UseCases;

public interface IRecipeRepository : IRepository<RecipeEntity>
{
    Task<List<RecipeEntity>> GetAllContainingIngredientAsync(Guid ingredientId);
}
