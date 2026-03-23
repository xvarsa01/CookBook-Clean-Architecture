using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Ingredient;
using CookBook.Clean.Core.IngredientRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

internal class GetIngredientListHandler (IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetIngredientListQuery, UseCaseResult<List<IngredientListModel>>>
{
    public async Task<UseCaseResult<List<IngredientListModel>>> Handle(GetIngredientListQuery request, CancellationToken cancellationToken)
    {
        var specification = new IngredientsBySpecification(request.Filter,  request.PagingOptions);
        var ingredients = await repository.GetListBySpecificationAsync(specification);

        var listModels = mapper.MapToListModels(ingredients).ToList();
        
        return UseCaseResult<List<IngredientListModel>>.Ok(listModels);
    }
}
