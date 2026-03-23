using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IIngredientMapper : IMapper
{
    IngredientListModel MapToListModel(IngredientEntity entity);
    IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientEntity> entities);
    IngredientDetailModel MapToDetailModel(IngredientEntity entity);
    
    IngredientEntity MapToEntity(CreateIngredientCommand request);
}
