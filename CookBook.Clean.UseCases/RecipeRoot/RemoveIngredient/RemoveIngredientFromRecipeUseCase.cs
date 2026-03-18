using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.RemoveIngredient;

public record RemoveIngredientFromRecipeUseCase(Guid RecipeId, Guid EntryId) : IRequest<UseCaseResult>;
