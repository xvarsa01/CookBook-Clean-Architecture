using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.Create;

namespace CookBook.Clean.UseCases.Mappers;

public interface IRecipeMapper
{
    RecipeListModel MapToListModel(RecipeEntity entity);
    IEnumerable<RecipeListModel> MapToListModels(IEnumerable<RecipeEntity> entities);
    RecipeDetailModel MapToDetailModel(RecipeEntity entity, List<IngredientEntity> usedIngredientDetails);
    
    RecipeEntity MapToEntity(CreateRecipeUseCase request);
}
