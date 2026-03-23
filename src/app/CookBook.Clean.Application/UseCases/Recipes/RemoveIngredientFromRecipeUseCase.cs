using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record RemoveIngredientFromRecipeUseCase(Guid RecipeId, Guid EntryId) : IRequest<UseCaseResult>;
