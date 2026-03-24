using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Mappers;

public class ManualIngredientMapper : IIngredientMapper
{
    public IngredientListModel MapToListModel(IngredientBase @base)
    {
        return new IngredientListModel
        {
            Id = @base.Id,
            Name = @base.Name,
            ImageUrl = @base.ImageUrl?.Value,
        };
    }

    public IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientBase> entities)
    {
        List<IngredientListModel> list = [];
        foreach (var entity in entities)
        {
            var listModel = MapToListModel(entity);
            list.Add(listModel);
        }
        return list;
    }

    public IngredientDetailModel MapToDetailModel(IngredientBase @base)
    {
        return new IngredientDetailModel
        {
            Id = @base.Id,
            Name = @base.Name,
            Description = @base.Description,
            ImageUrl = @base.ImageUrl?.Value,
        };
    }

    public IngredientBase MapToEntity(CreateIngredientCommand request)
    {
        var urlObjectResult = request.ImageUrl is not null
            ? ImageUrl.CreateObject(request.ImageUrl)
            : null;

        return IngredientBase.Create(request.Name,
            request.Description,
            urlObjectResult?.Value).Value;
    }
}
