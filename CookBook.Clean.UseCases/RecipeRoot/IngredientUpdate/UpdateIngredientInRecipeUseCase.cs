using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.IngredientUpdate;

public record UpdateIngredientInRecipeUseCase(Guid RecipeId, Guid EntryId, decimal NewAmount, MeasurementUnit NewUnit) : IRequest<UseCaseResult<UpdateIngredientInRecipeUseCaseResult>>;
