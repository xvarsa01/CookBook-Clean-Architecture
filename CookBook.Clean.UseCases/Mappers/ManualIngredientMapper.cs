using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases.IngredientRoot.Create;
using CookBook.Clean.UseCases.Models;

namespace CookBook.Clean.UseCases.Mappers;

public class ManualIngredientMapper : IIngredientMapper
{
    public IngredientListModel MapToListModel(IngredientEntity entity)
    {
        return new IngredientListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            ImageUrl = entity.ImageUrl,
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
            ImageUrl = entity.ImageUrl,
        };
    }

    public IngredientEntity MapToEntity(CreateIngredientUseCase request)
    {
        return new IngredientEntity(
            request.Name,
            request.Description,
            request.ImageUrl);
    }
}
