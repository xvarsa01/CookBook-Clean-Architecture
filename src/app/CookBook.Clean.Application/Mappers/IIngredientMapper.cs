using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Mappers;

public interface IIngredientMapper : IMapper
{
    IngredientListModel MapToListModel(Ingredient @base);
    IEnumerable<IngredientListModel> MapToListModels(IEnumerable<Ingredient> entities);
    IngredientDetailModel MapToDetailModel(Ingredient @base);
    
    Ingredient MapToEntity(CreateIngredientCommand request);
}
