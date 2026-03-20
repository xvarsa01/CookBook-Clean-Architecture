using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.Get;

public class GetRecipeHandler(IRepository<RecipeEntity> repository, IRepository<IngredientEntity> ingredientRepository, IRecipeMapper mapper) : IRequestHandler<GetRecipeQuery, UseCaseResult<RecipeDetailModel>>
{
    public async Task<UseCaseResult<RecipeDetailModel>> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
    {
        RecipeEntity? recipe = await repository.GetByIdAsync(request.Id);
        if (recipe is null)
        {
            return UseCaseResult<RecipeDetailModel>.NotFound("Recipe not found");
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
        return UseCaseResult<RecipeDetailModel>.Ok(recipeDetailModel);
    }
}
