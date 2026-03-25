using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.IngredientRoot;
using CookBook.CleanArch.Domain.IngredientRoot.ValueObjects;
using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.Errors;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Queries.Recipes;

public record GetRecipeDetailQuery(Guid Id) : IQuery<RecipeGetDetailResponse>;

internal class GetRecipeDetailQueryHandler(IRepository<Recipe, RecipeId> repository, IRepository<Ingredient, IngredientId> ingredientRepository) : IQueryHandler<GetRecipeDetailQuery, RecipeGetDetailResponse>
{
    public async Task<Result<RecipeGetDetailResponse>> Handle(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        Recipe? entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound<RecipeGetDetailResponse>(RecipeErrors.RecipeNotFoundError(new RecipeId(request.Id)));
        }

        var ingredientIds = entity.Ingredients.Select(i => i.IngredientId).ToList();
        var ingredientModels = ingredientRepository.Query()
            .Where(i => ingredientIds.Contains(i.Id))
            .Join(
                entity.Ingredients.AsQueryable(),
                ingredient => ingredient.Id,
                recipeIngredient => recipeIngredient.IngredientId,
                (ingredient, recipeIngredient) => new IngredientInRecipe(
                    recipeIngredient.Id,
                    recipeIngredient.IngredientId,
                    recipeIngredient.Amount,
                    recipeIngredient.Unit,
                    ingredient.Name,
                    ingredient.ImageUrl)
                )
            .ToList();
        
        var model = new RecipeGetDetailResponse(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.ImageUrl,
            entity.Duration,
            entity.Type,
            ingredientModels);
        
        return Result.Ok(model);
    }
}
