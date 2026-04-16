using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Recipes.Queries;

public record GetRecipeDetailQuery(RecipeId Id) : IQuery<RecipeResponse>;

internal class GetRecipeDetailQueryHandler(ICookBookDbContext dbContext) : IQueryHandler<GetRecipeDetailQuery, RecipeResponse>
{
    public async Task<Result<RecipeResponse>> Handle(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        var recipe = await dbContext
            .Recipes
            .Where(r => r.Id == request.Id)
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
                    ir.IngredientId,
                    ir.Amount,
                    ir.Unit,
                    ir.Ingredient.Name,
                    ir.Ingredient.ImageUrl
                )).ToList()))
                .FirstOrDefaultAsync(cancellationToken);

        return recipe == null
            ? Result.Failure<RecipeResponse>(RecipeErrors.RecipeNotFoundError(request.Id))
            : Result.Success(recipe);
    }
}
