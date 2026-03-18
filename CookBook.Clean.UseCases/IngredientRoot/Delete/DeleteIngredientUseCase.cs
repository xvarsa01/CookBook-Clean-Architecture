using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.Delete;

public record DeleteIngredientUseCase(Guid Id) : IRequest<UseCaseResult>;
