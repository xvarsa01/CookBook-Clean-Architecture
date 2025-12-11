using MediatR;

namespace CookBook.Clean.UseCases.Recipe.GetList;

public record GetListRecipeUseCase() : IRequest<UseCaseResult<GetListRecipeResult>>;