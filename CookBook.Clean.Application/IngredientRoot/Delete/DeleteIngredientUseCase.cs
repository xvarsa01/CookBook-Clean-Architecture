using MediatR;

namespace CookBook.Clean.Application.IngredientRoot.Delete;

public record DeleteIngredientUseCase(Guid Id) : IRequest<UseCaseResult>;
