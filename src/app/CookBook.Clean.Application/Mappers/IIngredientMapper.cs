using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IIngredientMapper : IMapper
{
    IngredientListModel MapToListModel(IngredientBase @base);
    IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientBase> entities);
    IngredientDetailModel MapToDetailModel(IngredientBase @base);
    
    IngredientBase MapToEntity(CreateIngredientCommand request);
}
