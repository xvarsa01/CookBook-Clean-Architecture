using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Get;

public record GetRecipeResult(RecipeDetailModel Recipe);

public class GetRecipeHandler(IRepository<RecipeEntity> repository, IRepository<IngredientEntity> ingredientRepository) : IRequestHandler<GetRecipeUseCase, UseCaseResult<GetRecipeResult>>
{
    public async Task<UseCaseResult<GetRecipeResult>> Handle(GetRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipe = await repository.GetByIdAsync(request.Id);
        if (recipe is null)
        {
            return UseCaseResult<GetRecipeResult>.NotFound("Recipe not found");
        }

        var ingredientIds = recipe.Ingredients.Select(i => i.IngredientId).ToList();
        List<IngredientEntity> ingredients = [];
        foreach (var ingredientId in ingredientIds)
        {
            var ingredient = await ingredientRepository.GetByIdAsync(ingredientId);
            if (ingredient != null)
            {
                ingredients.Add(ingredient);
            }
        }

        var ingredientsInRecipeModel = recipe.Ingredients.Select(i =>
        {
            var ingredient = ingredients.First(x => x.Id == i.IngredientId);
            return new IngredientInRecipeModel
            {
                Id = i.Id,
                IngredientId = i.IngredientId,
                Amount = i.Amount,
                Unit = i.Unit,
                Name = ingredient.Name,
                ImageUrl = ingredient.ImageUrl,
            };
        }).ToList();
        
        return UseCaseResult<GetRecipeResult>.Ok(new GetRecipeResult(
            new RecipeDetailModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                Ingredients = ingredientsInRecipeModel
            }));
    }
}
