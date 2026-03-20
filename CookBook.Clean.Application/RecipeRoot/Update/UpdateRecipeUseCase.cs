using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.Update;

public record UpdateRecipeUseCase(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl, TimeSpan? NewDuration, RecipeType? NewType) : IRequest<UseCaseResult<Guid>>;
