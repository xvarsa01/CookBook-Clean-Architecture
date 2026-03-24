using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeDetailQuery(Guid Id) : IQuery<RecipeGetDetailDto>;

internal class GetRecipeDetailQueryHandler(IRepository<RecipeEntity> repository, IRepository<IngredientEntity> ingredientRepository) : IQueryHandler<GetRecipeDetailQuery, RecipeGetDetailDto>
{
    public async Task<Result<RecipeGetDetailDto>> Handle(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        RecipeEntity? entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
        {
            return Result.NotFound<RecipeGetDetailDto>("Recipe not found");
        }

        var ingredientIds = entity.Ingredients.Select(i => i.IngredientId).ToList();
        var ingredientModels = ingredientRepository.Query()
            .Where(i => ingredientIds.Contains(i.Id))
            .Join(
                entity.Ingredients.AsQueryable(),
                ingredient => ingredient.Id,
                recipeIngredient => recipeIngredient.IngredientId,
                (ingredient, recipeIngredient) => new IngredientInRecipeModel
                {
                    Id = recipeIngredient.Id,
                    IngredientId = recipeIngredient.IngredientId,
                    Amount = recipeIngredient.Amount,
                    Unit = recipeIngredient.Unit,
                    Name = ingredient.Name,
                    ImageUrl = ingredient.ImageUrl
                })
            .ToList();
        
        var model = new RecipeGetDetailDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            Duration = entity.Duration,
            Type = entity.Type,
            Ingredients = ingredientModels
        };
        
        return Result.Ok(model);
    }
}
