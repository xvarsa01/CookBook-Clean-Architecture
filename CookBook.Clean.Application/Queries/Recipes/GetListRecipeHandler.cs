using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Recipe;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public class GetListRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<GetListRecipeQuery, UseCaseResult<List<RecipeListModel>>>
{
    public async Task<UseCaseResult<List<RecipeListModel>>> Handle(GetListRecipeQuery request, CancellationToken cancellationToken)
    {
        var specification = new RecipesBySpecification(request.filter,  request.PagingOptions);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        
        return UseCaseResult<List<RecipeListModel>>.Ok(listModels);
    }
}
