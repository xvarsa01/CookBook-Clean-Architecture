using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Specifications.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public class GetRecipeListByContainingIngredientIdHandler (IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<GetRecipeListByContainingIngredientIdQuery, UseCaseResult<List<RecipeListModel>>>
{
    public async Task<UseCaseResult<List<RecipeListModel>>> Handle(GetRecipeListByContainingIngredientIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new RecipesByContainingIngredientId(request.IngredientId);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        return UseCaseResult<List<RecipeListModel>>.Ok(listModels);
    }
}
