using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Delete;

public record DeleteIngredientUseCase(Guid Id) : IRequest<UseCaseResult>;
