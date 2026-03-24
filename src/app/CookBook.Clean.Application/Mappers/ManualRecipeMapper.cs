using System.Collections.ObjectModel;
using CookBook.Clean.Application.Commands.Recipes;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Mappers;

public class ManualRecipeMapper : IRecipeMapper
{
    public RecipeListModel MapToListModel(Recipe @base)
    {
        return new RecipeListModel
        {
            Id = @base.Id,
            Name = @base.Name.Value,
            RecipeType =  @base.Type,
            Duration = @base.Duration.Value,
            ImageUrl = @base.ImageUrl?.Value,
            CreatedAt = @base.CreatedAt,
            ModifiedAt = @base.ModifiedAt
        };
    }

    public IEnumerable<RecipeListModel> MapToListModels(IEnumerable<Recipe> entities)
    {
        List<RecipeListModel> list = [];
        foreach (var entity in entities)
        {
            var listModel = MapToListModel(entity);
            list.Add(listModel);
        }
        return list;
    }

    public RecipeDetailModel MapToDetailModel(Recipe @base, List<Ingredient> usedIngredientDetails)
    {
        List<IngredientInRecipeModel> ingredients = [];
        foreach (var ingredient in @base.Ingredients)
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
            Id = @base.Id,
            Name = @base.Name,
            Description = @base.Description,
            ImageUrl = @base.ImageUrl?.Value,
            Duration = @base.Duration.Value,
            Type = @base.Type,
            CreatedAt = @base.CreatedAt,
            ModifiedAt = @base.ModifiedAt,
            Ingredients = new ObservableCollection<IngredientInRecipeModel>(ingredients)
        };
    }

    public Recipe MapToEntity(CreateRecipeCommand request)
    {
        var url = request.ImageUrl is not null ? ImageUrl.CreateObject(request.ImageUrl) : null;
        
        var result = Recipe.Create(RecipeName.CreateObject(request.Name).Value,
            request.Description,
            url?.Value,
            RecipeDuration.CreateObject(request.Duration).Value,
            request.RecipeType);
        
        return result.Value;
    }
}
