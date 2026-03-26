using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipe.Errors;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Queries.Recipes;

public record GetRecipeDetailQuery(RecipeId Id) : IQuery<RecipeResponse>;

internal class GetRecipeDetailQueryHandler(ICookBookDbContext dbContext) : IQueryHandler<GetRecipeDetailQuery, RecipeResponse>
{
    public async Task<Result<RecipeResponse>> Handle(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        var recipe = await dbContext
            .Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(i => i.Ingredient)
            .Select(recipe => new RecipeResponse(
                recipe.Id,
                recipe.Name,
                recipe.Description,
                recipe.ImageUrl,
                recipe.Duration,
                recipe.Type,
                recipe.Ingredients.Select(ir => new RecipeIngredientResponse(
                    ir.Id,
                    ir.IngredientId.Id,
                    ir.Amount,
                    ir.Unit,
                    ir.Ingredient.Name,
                    ir.Ingredient.ImageUrl
                )).ToList()))
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        return recipe == null
            ? Result.NotFound<RecipeResponse>(RecipeErrors.RecipeNotFoundError(request.Id))
            : Result.Ok(recipe);
    }
}
