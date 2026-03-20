using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Ingredient;
using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Application.Specifications.Recipe;
using MediatR;

namespace CookBook.Clean.Application.IngredientRoot.GetList;

public class GetListIngredientHandler (IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetListIngredientQuery, UseCaseResult<List<IngredientListModel>>>
{
    public async Task<UseCaseResult<List<IngredientListModel>>> Handle(GetListIngredientQuery request, CancellationToken cancellationToken)
    {
        var specification = new IngredientsBySpecification(request.filter,  request.PagingOptions);
        var ingredients = await repository.GetListBySpecificationAsync(specification);

        var listModels = mapper.MapToListModels(ingredients).ToList();
        
        return UseCaseResult<List<IngredientListModel>>.Ok(listModels);
    }
}
