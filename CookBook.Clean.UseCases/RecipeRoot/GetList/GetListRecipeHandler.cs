using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.UseCases.ExternalInterfaces;
using CookBook.Clean.UseCases.Mappers;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.GetList;

public record GetListRecipeResult(List<RecipeListModel> Recipes);

public class GetListRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<GetListRecipeUseCase, UseCaseResult<GetListRecipeResult>>
{
    public async Task<UseCaseResult<GetListRecipeResult>> Handle(GetListRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipes = await repository.GetAllAsync();
        
        var listModels = mapper.MapToListModels(recipes).ToList();
        
        return UseCaseResult<GetListRecipeResult>.Ok(new GetListRecipeResult(listModels));
    }
}
