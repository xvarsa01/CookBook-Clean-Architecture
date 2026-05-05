using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Application.Recipes.Queries;

public record GetRecipeDetailQuery(RecipeId Id) : IQuery<RecipeResponse>;

internal class GetRecipeDetailQueryHandler(ICookBookDbContext dbContext, IRecipeRepository recipeRepository) : IQueryHandler<GetRecipeDetailQuery, RecipeResponse>
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
    
    /// <summary>
    /// General rule in Clean Architecture : Application layer should not care which DB is used in infrastructure layer.
    /// It can be SQL database with EF Core engine, NoSQL, InMemory database, or any other you wish...
    ///
    /// Method above breaks this rule!
    /// It directly depends on `ICookBookDbContext` which is an EF Core abstraction, and it uses EF Core specific
    /// features like `Include`, `ThenInclude` and `FirstOrDefaultAsync`.
    ///
    /// Solution is to add all `Get` methods (such as `GetRecipeWithIngredientsByIdAsync`) into repositories and use 
    ///  only `IRepository` or `ISomethingRepository` in application layer.
    ///
    /// Method above is more practical, if you know which DB you will use, and you don't want to add more methods
    /// to repositories, but it is less flexible and less testable. It is also better for performance.
    ///
    /// Method below is better for quick development if you don't know which DB will be used in the end. Or you plan to
    /// change DB in the future. Using this method during change of DB you would change only repository implementations,
    /// and not the application layer. It is also better for testability, because you can easily mock repositories.
    /// </summary>
    public async Task<Result<RecipeResponse>> HandleUnusedAlternative(GetRecipeDetailQuery request, CancellationToken cancellationToken)
    {
        var recipe = await recipeRepository.GetRecipeWithIngredientsByIdAsync(request.Id);
        if (recipe == null)
            return Result.Failure<RecipeResponse>(RecipeErrors.RecipeNotFoundError(request.Id));

        var response = new RecipeResponse(
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
            )).ToList());

        return Result.Success(response);
    }
}
