using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.ExternalInterfaces;

public interface IRecipeRepository : IRepository<RecipeEntity>
{
    Task<List<RecipeEntity>> GetAllContainingIngredientAsync(Guid ingredientId);
}
