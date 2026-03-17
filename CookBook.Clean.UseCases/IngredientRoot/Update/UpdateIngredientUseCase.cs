using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Update;

public record UpdateIngredientUseCase(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl) : IRequest<UseCaseResult<Guid>>;
