using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Ingredient;
using CookBook.Clean.Application.Specifications.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IQuery<List<RecipeListModel>>;

internal class GetRecipeListByContainingIngredientNameQueryHandler (IRepository<RecipeBase> repository, IRepository<IngredientBase> ingredientRepository, IRecipeMapper mapper)
    : IQueryHandler<GetRecipeListByContainingIngredientNameQuery, List<RecipeListModel>>
{
    public async Task<Result<List<RecipeListModel>>> Handle(GetRecipeListByContainingIngredientNameQuery request, CancellationToken cancellationToken)
    {
        var ingredientFilter = new IngredientFilter { Name = request.IngredientNameSubstring };
        var ingredientSpecification = new IngredientsBySpecification(ingredientFilter, null);
        
        var matchedIngredients = await ingredientRepository.GetListBySpecificationAsync(ingredientSpecification);
        var matchedIngredientIds = matchedIngredients.Select(i => i.Id).ToList();
        
        var specification = new RecipesByContainingIngredientIds(matchedIngredientIds);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        return Result.Ok(listModels);
    }
}
