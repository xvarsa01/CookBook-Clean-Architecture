using System.Collections.ObjectModel;
using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
using CookBook.Clean.UseCases.Recipe;
using CookBook.Clean.UseCases.Recipe.Create;

namespace CookBook.Clean.UseCases.Mappers;

public class ManualRecipeMapper : IRecipeMapper
{
    public RecipeListModel MapToListModel(RecipeEntity entity)
    {
        return new RecipeListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            RecipeType =  entity.Type,
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
                Amount = ingredient.Amount,
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
            // Duration = entity.Duration,
            Type = entity.Type,
            ImageUrl = entity.ImageUrl,
            Ingredients = new ObservableCollection<IngredientInRecipeModel>(ingredients)
        };
    }

    public RecipeEntity MapToEntity(CreateRecipeUseCase request)
    {
        return new RecipeEntity(
            request.Name,
            request.Description,
            request.ImageUrl,
            request.RecipeType);
    }
}
