using CookBook.Clean.Core.Ingredient;
using CookBook.Clean.UseCases.Mappers;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.GetList;

public record GetListIngredientResult(List<IngredientListModel> Ingredients);

public class GetListIngredientHandler (IRepository<IngredientEntity> repository, IIngredientMapper mapper) : IRequestHandler<GetListIngredientUseCase, UseCaseResult<GetListIngredientResult>>
{
    public async Task<UseCaseResult<GetListIngredientResult>> Handle(GetListIngredientUseCase request, CancellationToken cancellationToken)
    {
        var ingredients = await repository.GetAllAsync();
        
        var listModels = mapper.MapToListModels(ingredients).ToList();
        
        return UseCaseResult<GetListIngredientResult>.Ok(new GetListIngredientResult(listModels));
    }
}
