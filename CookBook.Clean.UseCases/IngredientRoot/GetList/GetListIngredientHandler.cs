using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.GetList;

public class GetListIngredientHandler (IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetListIngredientQuery, UseCaseResult<List<IngredientListModel>>>
{
    public async Task<UseCaseResult<List<IngredientListModel>>> Handle(GetListIngredientQuery request, CancellationToken cancellationToken)
    {
        var ingredients = request.PagingOptions is null
            ? await repository.GetAllAsync()
            : await repository.GetAllAsync(request.PagingOptions.PageIndex, request.PagingOptions.PageSize);

        var listModels = mapper.MapToListModels(ingredients).ToList();
        
        return UseCaseResult<List<IngredientListModel>>.Ok(listModels);
    }
}
