using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.IngredientUpdate;

public record UpdateIngredientInRecipeUseCase(Guid RecipeId, Guid EntryId, decimal NewAmount, MeasurementUnit NewUnit) : IRequest<UseCaseResult>;
