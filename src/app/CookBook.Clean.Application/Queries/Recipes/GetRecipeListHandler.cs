using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

internal class GetRecipeListHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<GetRecipeListQuery, Result<List<RecipeListModel>>>
{
    public async Task<Result<List<RecipeListModel>>> Handle(GetRecipeListQuery request, CancellationToken cancellationToken)
    {
        var specification = new RecipesBySpecification(request.Filter,  request.PagingOptions);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        
        return Result<List<RecipeListModel>>.Ok(listModels);
    }
}
