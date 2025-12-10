using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.GetList;

public record GetListIngredientResult(List<IngredientListModel> Ingredients);

public class GetListIngredientHandler (IRepository<IngredientEntity> repository) : IRequestHandler<GetListIngredientUseCase, UseCaseResult<GetListIngredientResult>>
{
    public async Task<UseCaseResult<GetListIngredientResult>> Handle(GetListIngredientUseCase request, CancellationToken cancellationToken)
    {
        var ingredients = await repository.GetAllAsync();
        
        var list =  ingredients.Select(i => new IngredientListModel()
        {
            Id = i.Id,
            Name = i.Name,
            ImageUrl = i.ImageUrl
        }).ToList();
        return UseCaseResult<GetListIngredientResult>.Ok(new GetListIngredientResult(list));
    }
}