using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record AddIngredientToRecipeUseCase(Guid RecipeId, Guid IngredientId, decimal Amount, MeasurementUnit Unit) : IRequest<UseCaseResult<Guid>>;
