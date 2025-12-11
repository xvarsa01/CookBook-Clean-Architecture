using MediatR;

namespace CookBook.Clean.UseCases.Recipe.RemoveIngredient;

public record RemoveIngredientFromRecipeUseCase(Guid RecipeId, Guid EntryId) : IRequest<UseCaseResult<RemoveIngredientFromRecipeResult>>;
