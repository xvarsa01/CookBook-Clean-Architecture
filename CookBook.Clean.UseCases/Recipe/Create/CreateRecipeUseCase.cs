using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Create;

public record CreateRecipeUseCase(string Name, string? Description, string? ImageUrl) : IRequest<UseCaseResult<CreateRecipeResult>>;
