using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.Delete;

public record DeleteRecipeUseCase(Guid Id) : IRequest<UseCaseResult>;
