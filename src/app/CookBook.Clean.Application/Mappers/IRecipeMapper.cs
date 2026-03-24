using CookBook.Clean.Application.Commands.Recipes;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IRecipeMapper : IMapper
{
    RecipeListModel MapToListModel(Recipe @base);
    IEnumerable<RecipeListModel> MapToListModels(IEnumerable<Recipe> entities);
    RecipeDetailModel MapToDetailModel(Recipe @base, List<Ingredient> usedIngredientDetails);
    
    Recipe MapToEntity(CreateRecipeCommand request);
}
