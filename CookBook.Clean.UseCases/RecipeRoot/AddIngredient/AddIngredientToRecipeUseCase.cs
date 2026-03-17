using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.AddIngredient;

public record AddIngredientToRecipeUseCase(Guid RecipeId, Guid IngredientId, decimal Amount, MeasurementUnit Unit) : IRequest<UseCaseResult<AddIngredientToRecipeResult>>;
