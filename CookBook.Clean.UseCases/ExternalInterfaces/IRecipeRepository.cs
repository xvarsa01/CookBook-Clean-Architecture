using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.UseCases.ExternalInterfaces;

public interface IRecipeRepository : IRepository<RecipeEntity>
{
    Task<List<RecipeEntity>> GetAllContainingIngredientAsync(Guid ingredientId);
}
