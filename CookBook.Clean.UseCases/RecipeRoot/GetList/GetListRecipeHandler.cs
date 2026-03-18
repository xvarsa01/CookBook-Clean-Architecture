using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public class GetListRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<GetListRecipeQuery, UseCaseResult<List<RecipeListModel>>>
{
    public async Task<UseCaseResult<List<RecipeListModel>>> Handle(GetListRecipeQuery request, CancellationToken cancellationToken)
    {
        var recipes = request.PagingOptions is null
            ? await repository.GetAllAsync()
            : await repository.GetAllAsync(request.PagingOptions.PageIndex, request.PagingOptions.PageSize);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        
        return UseCaseResult<List<RecipeListModel>>.Ok(listModels);
    }
}
