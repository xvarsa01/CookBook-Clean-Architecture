using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using CookBook.Clean.UseCases.Specifications.Ingredient;
using CookBook.Clean.UseCases.Specifications.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.GetList;

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
