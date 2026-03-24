using CookBook.Clean.Application.Commands.Recipes;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IRecipeMapper : IMapper
{
    RecipeListModel MapToListModel(RecipeBase @base);
    IEnumerable<RecipeListModel> MapToListModels(IEnumerable<RecipeBase> entities);
    RecipeDetailModel MapToDetailModel(RecipeBase @base, List<IngredientBase> usedIngredientDetails);
    
    RecipeBase MapToEntity(CreateRecipeCommand request);
}
