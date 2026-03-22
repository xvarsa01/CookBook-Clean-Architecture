using System.Collections.ObjectModel;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.UseCases.Recipes;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

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
            ImageUrl = entity.ImageUrl,
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
                ImageUrl =  ingredientDetail.ImageUrl,
            };

            ingredients.Add(ingredientInRecipeModel);
        }
        
        return new RecipeDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Duration = entity.Duration.Value,
            Type = entity.Type,
            ImageUrl = entity.ImageUrl,
            Ingredients = new ObservableCollection<IngredientInRecipeModel>(ingredients)
        };
    }

    public RecipeEntity MapToEntity(CreateRecipeUseCase request)
    {
        return new RecipeEntity(
            new RecipeName(request.Name),
            request.Description,
            request.ImageUrl,
            new RecipeDuration(request.Duration),
            request.RecipeType);
    }
}
