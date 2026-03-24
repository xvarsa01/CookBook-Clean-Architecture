using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Application.Specifications.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListQuery(RecipeFilter Filter, PagingOptions? PagingOptions = null) : IQuery<List<RecipeListModel>>;

internal class GetRecipeListQueryHandler(IRepository<Recipe> repository, IRecipeMapper mapper) : IQueryHandler<GetRecipeListQuery, List<RecipeListModel>>
{
    public async Task<Result<List<RecipeListModel>>> Handle(GetRecipeListQuery request, CancellationToken cancellationToken)
    {
        var specification = new RecipesBySpecification(request.Filter,  request.PagingOptions);
        var recipes = await repository.GetListBySpecificationAsync(specification);
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        
        return Result.Ok(listModels);
    }
}
