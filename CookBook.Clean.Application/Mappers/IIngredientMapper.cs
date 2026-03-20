using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.UseCases.Ingredients;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IIngredientMapper : IMapper
{
    IngredientListModel MapToListModel(IngredientEntity entity);
    IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientEntity> entities);
    IngredientDetailModel MapToDetailModel(IngredientEntity entity);
    
    IngredientEntity MapToEntity(CreateIngredientUseCase request);
}
