using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Specifications.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

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
