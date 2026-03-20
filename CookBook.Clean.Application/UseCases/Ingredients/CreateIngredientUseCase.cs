using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;

public record CreateIngredientUseCase(string Name, string? Description, string? ImageUrl) : IRequest<UseCaseResult<Guid>>;
