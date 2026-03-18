using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.Delete;

public record DeleteRecipeUseCase(Guid Id) : IRequest<UseCaseResult>;
