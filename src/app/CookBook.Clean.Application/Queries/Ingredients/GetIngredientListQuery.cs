using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Ingredient;
using CookBook.Clean.Core;
using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientListQuery(IngredientFilter Filter, PagingOptions? PagingOptions = null) : IQuery<List<IngredientListModel>>;

internal class GetIngredientListQueryHandler (IRepository<Ingredient> repository, IIngredientMapper mapper) : IQueryHandler<GetIngredientListQuery, List<IngredientListModel>>
{
    public async Task<Result<List<IngredientListModel>>> Handle(GetIngredientListQuery request, CancellationToken cancellationToken)
    {
        var specification = new IngredientsBySpecification(request.Filter,  request.PagingOptions);
        var ingredients = await repository.GetListBySpecificationAsync(specification);

        var listModels = mapper.MapToListModels(ingredients).ToList();
        
        return Result.Ok(listModels);
    }
}
