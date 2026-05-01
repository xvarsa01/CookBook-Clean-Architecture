using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface IRecipeRepository : IRepository<Recipe, RecipeId>
{
    int GetRecipeCountByContainingIngredientId(IngredientId ingredientId);
    
    Task<Recipe?> GetRecipeWithIngredientsByIdAsync(RecipeId id); // this is unused in current application, check `GetRecipeDetailQuery` class for explanation
}
