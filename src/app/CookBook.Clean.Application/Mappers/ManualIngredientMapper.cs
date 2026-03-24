using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Mappers;

public class ManualIngredientMapper : IIngredientMapper
{
    public IngredientListModel MapToListModel(Ingredient @base)
    {
        return new IngredientListModel
        {
            Id = @base.Id,
            Name = @base.Name,
            ImageUrl = @base.ImageUrl?.Value,
        };
    }

    public IEnumerable<IngredientListModel> MapToListModels(IEnumerable<Ingredient> entities)
    {
        List<IngredientListModel> list = [];
        foreach (var entity in entities)
        {
            var listModel = MapToListModel(entity);
            list.Add(listModel);
        }
        return list;
    }

    public IngredientDetailModel MapToDetailModel(Ingredient @base)
    {
        return new IngredientDetailModel
        {
            Id = @base.Id,
            Name = @base.Name,
            Description = @base.Description,
            ImageUrl = @base.ImageUrl?.Value,
        };
    }

    public Ingredient MapToEntity(CreateIngredientCommand request)
    {
        var urlObjectResult = request.ImageUrl is not null
            ? ImageUrl.CreateObject(request.ImageUrl)
            : null;

        return Ingredient.Create(request.Name,
            request.Description,
            urlObjectResult?.Value).Value;
    }
}
