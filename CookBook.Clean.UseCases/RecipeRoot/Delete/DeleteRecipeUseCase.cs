using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Delete;

public record DeleteRecipeUseCase(Guid Id) : IRequest<UseCaseResult>;
