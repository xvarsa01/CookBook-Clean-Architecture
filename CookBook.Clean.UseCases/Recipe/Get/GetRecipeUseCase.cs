using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Get;

public record GetRecipeUseCase(Guid Id) : IRequest<UseCaseResult<GetRecipeResult>>;