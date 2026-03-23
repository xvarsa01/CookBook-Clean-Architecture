using CookBook.Clean.Application.Commands.Ingredients;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Mappers;

public class ManualIngredientMapper : IIngredientMapper
{
    public IngredientListModel MapToListModel(IngredientEntity entity)
    {
        return new IngredientListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            ImageUrl = entity.ImageUrl?.Value,
        };
    }

    public IEnumerable<IngredientListModel> MapToListModels(IEnumerable<IngredientEntity> entities)
    {
        List<IngredientListModel> list = [];
        foreach (var entity in entities)
        {
            var listModel = MapToListModel(entity);
            list.Add(listModel);
        }
        return list;
    }

    public IngredientDetailModel MapToDetailModel(IngredientEntity entity)
    {
        return new IngredientDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl?.Value,
        };
    }

    public IngredientEntity MapToEntity(CreateIngredientCommand request)
    {
        var urlObjectResult = request.ImageUrl is not null
            ? ImageUrl.CreateObject(request.ImageUrl)
            : null;

        return IngredientEntity.Create(request.Name,
            request.Description,
            urlObjectResult?.Value).Value;
    }
}
