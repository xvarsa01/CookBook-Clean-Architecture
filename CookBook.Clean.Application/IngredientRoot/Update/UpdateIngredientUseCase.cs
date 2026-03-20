using MediatR;

namespace CookBook.Clean.Application.IngredientRoot.Update;

public record UpdateIngredientUseCase(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl) : IRequest<UseCaseResult<Guid>>;
