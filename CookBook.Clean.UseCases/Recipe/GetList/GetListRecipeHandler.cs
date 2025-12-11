using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.GetList;

public record GetListRecipeResult(List<RecipeListModel> Recipes);

public class GetListRecipeHandler(IRepository<RecipeEntity> repository) : IRequestHandler<GetListRecipeUseCase, UseCaseResult<GetListRecipeResult>>
{
    public async Task<UseCaseResult<GetListRecipeResult>> Handle(GetListRecipeUseCase request, CancellationToken cancellationToken)
    {
        var recipes = await repository.GetAllAsync();
        var list = recipes.Select(r => new RecipeListModel
        {
            Id = r.Id,
            Name = r.Name,
            ImageUrl = r.ImageUrl
        }).ToList();
        return UseCaseResult<GetListRecipeResult>.Ok(new GetListRecipeResult(list));
    }
}
