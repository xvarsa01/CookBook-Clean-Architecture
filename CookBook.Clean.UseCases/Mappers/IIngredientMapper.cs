using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases.Ingredient;
using CookBook.Clean.UseCases.Ingredient.Create;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.Create;

namespace CookBook.Clean.UseCases.Mappers;

public interface IIngredientMapper
{
    IngredientListModel MapToListModel(IngredientEntity entity);
    IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientEntity> entities);
    IngredientDetailModel MapToDetailModel(IngredientEntity entity);
    
    IngredientEntity MapToEntity(CreateIngredientUseCase request);
}
