using CookBook.Clean.Core.RecipeRoot;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record UpdateIngredientInRecipeUseCase(Guid RecipeId, Guid EntryId, decimal NewAmount, MeasurementUnit NewUnit) : IRequest<UseCaseResult>;
