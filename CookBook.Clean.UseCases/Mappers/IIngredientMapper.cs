using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases.IngredientRoot.Create;
using CookBook.Clean.UseCases.Models;

namespace CookBook.Clean.UseCases.Mappers;

public interface IIngredientMapper
{
    IngredientListModel MapToListModel(IngredientEntity entity);
    IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientEntity> entities);
    IngredientDetailModel MapToDetailModel(IngredientEntity entity);
    
    IngredientEntity MapToEntity(CreateIngredientUseCase request);
}
