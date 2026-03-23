using System.Collections.ObjectModel;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.UseCases.Recipes;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Mappers;

public class ManualRecipeMapper : IRecipeMapper
{
    public RecipeListModel MapToListModel(RecipeEntity entity)
    {
        return new RecipeListModel
        {
            Id = entity.Id,
            Name = entity.Name.Value,
            RecipeType =  entity.Type,
            Duration = entity.Duration.Value,
            ImageUrl = entity.ImageUrl?.Value,
            CreatedAt = entity.CreatedAt,
            ModifiedAt = entity.ModifiedAt
        };
    }

    public IEnumerable<RecipeListModel> MapToListModels(IEnumerable<RecipeEntity> entities)
    {
        List<RecipeListModel> list = [];
        foreach (var entity in entities)
        {
            var listModel = MapToListModel(entity);
            list.Add(listModel);
        }
        return list;
    }

    public RecipeDetailModel MapToDetailModel(RecipeEntity entity, List<IngredientEntity> usedIngredientDetails)
    {
        List<IngredientInRecipeModel> ingredients = [];
        foreach (var ingredient in entity.Ingredients)
        {
            var ingredientDetail = usedIngredientDetails.First(i => i.Id == ingredient.IngredientId);
            
            var ingredientInRecipeModel = new IngredientInRecipeModel
            {
                Id = ingredient.Id,
                IngredientId = ingredient.IngredientId,
                Amount = ingredient.Amount.Value,
                Unit = ingredient.Unit,
                Name = ingredientDetail.Name,
                ImageUrl =  ingredientDetail.ImageUrl?.Value,
            };

            ingredients.Add(ingredientInRecipeModel);
        }
        
        return new RecipeDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl?.Value,
            Duration = entity.Duration.Value,
            Type = entity.Type,
            CreatedAt = entity.CreatedAt,
            ModifiedAt = entity.ModifiedAt,
            Ingredients = new ObservableCollection<IngredientInRecipeModel>(ingredients)
        };
    }

    public RecipeEntity MapToEntity(CreateRecipeCommand request)
    {
        var url = request.ImageUrl is not null ? ImageUrl.CreateObject(request.ImageUrl) : null;
        
        var result = RecipeEntity.Create(RecipeName.CreateObject(request.Name).Value,
            request.Description,
            url?.Value,
            RecipeDuration.CreateObject(request.Duration).Value,
            request.RecipeType);
        
        return result.Value;
    }
}
