using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Infrastructure.Repositories;

public class InMemoryRecipeRepository : InMemoryRepository<Recipe, RecipeId>, IRecipeRepository
{
    public int GetRecipeCountByContainingIngredientId(IngredientId ingredientId)
    {
        return Store.Values.Count(recipe => recipe.Ingredients.Any(ri => ri.IngredientId == ingredientId));
    }
}

