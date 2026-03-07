using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.Create;

namespace CookBook.Clean.UseCases.Mappers;

public interface IRecipeMapper
{
    RecipeListModel MapToListModel(RecipeEntity entity);
    IEnumerable<RecipeListModel> MapToListModels(IEnumerable<RecipeEntity> entities);
    RecipeDetailModel MapToDetailModel(RecipeEntity entity, List<IngredientEntity> usedIngredientDetails);
    RecipeEntity MapToEntity(CreateRecipeUseCase useCase);
}
