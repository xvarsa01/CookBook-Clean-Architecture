using CookBook.CleanArch.Application.Models;
using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IQuery<List<RecipeGetListResponse>>;

internal class GetRecipeListByContainingIngredientIdQueryHandler (IRepository<Recipe, RecipeId> repository) : IQueryHandler<GetRecipeListByContainingIngredientIdQuery, List<RecipeGetListResponse>>
{
    public Task<Result<List<RecipeGetListResponse>>> Handle(GetRecipeListByContainingIngredientIdQuery request, CancellationToken cancellationToken)
    {
        var queryable = repository.Query();
        
        queryable = queryable.Where(r => r.Ingredients.Any(i => i.IngredientId == request.IngredientId));
        
        var result = queryable.Select(i => new RecipeGetListResponse(i.Id, i.Name, i.ImageUrl)).ToList();
        
        return Task.FromResult(Result.Ok(result));
    }
}
