using MediatR;

namespace CookBook.Clean.Application.IngredientRoot.Create;

public record CreateIngredientUseCase(string Name, string? Description, string? ImageUrl) : IRequest<UseCaseResult<Guid>>;
