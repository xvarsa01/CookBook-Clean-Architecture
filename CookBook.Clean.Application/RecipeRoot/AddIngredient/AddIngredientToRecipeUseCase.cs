using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.AddIngredient;

public record AddIngredientToRecipeUseCase(Guid RecipeId, Guid IngredientId, decimal Amount, MeasurementUnit Unit) : IRequest<UseCaseResult<Guid>>;
