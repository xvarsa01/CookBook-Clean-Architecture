using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

internal class GetRecipeDetailHandler(IRepository<RecipeEntity> repository, IRepository<IngredientEntity> ingredientRepository, IRecipeMapper mapper) : IRequestHandler<GetRecipeDetailQuery, Result<RecipeDetailModel>>
{
    public async Task<Result<RecipeDetailModel>> Handle(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        RecipeEntity? recipe = await repository.GetByIdAsync(request.Id);
        if (recipe is null)
        {
            return Result.NotFound<RecipeDetailModel>("Recipe not found");
        }

        var ingredientIds = recipe.Ingredients.Select(i => i.IngredientId).ToList();
        List<IngredientEntity> usedIngredients = [];
        foreach (var ingredientId in ingredientIds)
        {
            var ingredient = await ingredientRepository.GetByIdAsync(ingredientId);
            if (ingredient != null)
            {
                usedIngredients.Add(ingredient);
            }
        }

        var recipeDetailModel = mapper.MapToDetailModel(recipe, usedIngredients);
        return Result.Ok(recipeDetailModel);
    }
}
