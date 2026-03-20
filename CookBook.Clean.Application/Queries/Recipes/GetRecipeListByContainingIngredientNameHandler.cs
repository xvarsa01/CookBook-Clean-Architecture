using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Ingredient;
using CookBook.Clean.Application.Specifications.Recipe;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public class GetRecipeListByContainingIngredientNameHandler (IRepository<RecipeEntity> repository, IRepository<IngredientEntity> ingredientRepository, IRecipeMapper mapper) : IRequestHandler<GetRecipeListByContainingIngredientNameQuery, UseCaseResult<List<RecipeListModel>>>
{
    public async Task<UseCaseResult<List<RecipeListModel>>> Handle(GetRecipeListByContainingIngredientNameQuery request, CancellationToken cancellationToken)
    {
        var ingredientFilter = new IngredientFilter { Name = request.IngredientNameSubstring };
        var ingredientSpecification = new IngredientsBySpecification(ingredientFilter, null);
        
        var matchedIngredients = await ingredientRepository.GetListBySpecificationAsync(ingredientSpecification);
        var matchedIngredientIds = matchedIngredients.Select(i => i.Id).ToList();
        
        var specification = new RecipesByContainingIngredientIds(matchedIngredientIds);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        return UseCaseResult<List<RecipeListModel>>.Ok(listModels);
    }
}
