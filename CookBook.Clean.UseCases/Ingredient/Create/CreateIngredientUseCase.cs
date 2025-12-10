using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Create;

public record CreateIngredientUseCase(string Name, string? Description, string? ImageUrl) : IRequest<Guid>;
