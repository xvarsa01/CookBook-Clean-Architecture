using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.RemoveIngredient;

public record RemoveIngredientFromRecipeUseCase(Guid RecipeId, Guid EntryId) : IRequest<UseCaseResult>;
