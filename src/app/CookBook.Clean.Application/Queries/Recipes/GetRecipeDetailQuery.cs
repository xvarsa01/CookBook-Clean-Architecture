using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeDetailQuery(Guid Id) : IQuery<RecipeDetailModel>;

internal class GetRecipeDetailQueryHandler(IRepository<Recipe> repository, IRepository<Ingredient> ingredientRepository, IRecipeMapper mapper) : IQueryHandler<GetRecipeDetailQuery, RecipeDetailModel>
{
    public async Task<Result<RecipeDetailModel>> Handle(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        Recipe? recipe = await repository.GetByIdAsync(request.Id);
        if (recipe is null)
        {
            return Result.NotFound<RecipeDetailModel>("Recipe not found");
        }

        var ingredientIds = recipe.Ingredients.Select(i => i.IngredientId).ToList();
        List<Ingredient> usedIngredients = [];
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
