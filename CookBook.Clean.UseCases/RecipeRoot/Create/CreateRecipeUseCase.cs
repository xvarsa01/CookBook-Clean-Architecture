using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.Create;

public record CreateRecipeUseCase(string Name, string? Description, string? ImageUrl, TimeSpan Duration, RecipeType RecipeType) : IRequest<UseCaseResult<Guid>>;
