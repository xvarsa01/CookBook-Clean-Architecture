using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IQuery<List<RecipeListModel>>;

internal class GetRecipeListByContainingIngredientIdQueryHandler (IRepository<RecipeBase> repository, IRecipeMapper mapper) : IQueryHandler<GetRecipeListByContainingIngredientIdQuery, List<RecipeListModel>>
{
    public async Task<Result<List<RecipeListModel>>> Handle(GetRecipeListByContainingIngredientIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new RecipesByContainingIngredientId(request.IngredientId);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        return Result.Ok(listModels);
    }
}
