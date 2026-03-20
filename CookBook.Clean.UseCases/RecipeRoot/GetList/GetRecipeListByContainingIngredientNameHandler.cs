using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Filters;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Specifications.Ingredient;
using CookBook.Clean.UseCases.Specifications.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

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
