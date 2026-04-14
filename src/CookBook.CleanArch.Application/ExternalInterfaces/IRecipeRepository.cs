using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface IRecipeRepository : IRepository<Recipe, RecipeId>
{
    int GetRecipeCountByContainingIngredientId(IngredientId ingredientId);
}
