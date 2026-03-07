using CookBook.Clean.Core.Recipe;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Create;

public record CreateRecipeUseCase(string Name, string? Description, string? ImageUrl, TimeSpan Duration, RecipeType RecipeType) : IRequest<UseCaseResult<CreateRecipeResult>>;
