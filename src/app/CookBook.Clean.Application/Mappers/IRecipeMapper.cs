using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.UseCases.Recipes;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IRecipeMapper : IMapper
{
    RecipeListModel MapToListModel(RecipeEntity entity);
    IEnumerable<RecipeListModel> MapToListModels(IEnumerable<RecipeEntity> entities);
    RecipeDetailModel MapToDetailModel(RecipeEntity entity, List<IngredientEntity> usedIngredientDetails);
    
    RecipeEntity MapToEntity(CreateRecipeCommand request);
}
