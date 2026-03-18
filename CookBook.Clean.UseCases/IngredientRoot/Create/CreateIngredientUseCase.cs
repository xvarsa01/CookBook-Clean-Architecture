using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.Create;

public record CreateIngredientUseCase(string Name, string? Description, string? ImageUrl) : IRequest<UseCaseResult<Guid>>;
