using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Get;

public record GetRecipeResult(RecipeDetailModel Recipe);

public class GetRecipeHandler(IRepository<RecipeEntity> repository, IRepository<IngredientEntity> ingredientRepository, IRecipeMapper mapper) : IRequestHandler<GetRecipeUseCase, UseCaseResult<GetRecipeResult>>
{
    public async Task<UseCaseResult<GetRecipeResult>> Handle(GetRecipeUseCase request, CancellationToken cancellationToken)
    {
        RecipeEntity? recipe = await repository.GetByIdAsync(request.Id);
        if (recipe is null)
        {
            return UseCaseResult<GetRecipeResult>.NotFound("Recipe not found");
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
        return UseCaseResult<GetRecipeResult>.Ok(new GetRecipeResult(recipeDetailModel));
    }
}
