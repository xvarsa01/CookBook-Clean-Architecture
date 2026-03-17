using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Get;

public record GetIngredientUseCase(Guid Id) : IRequest<UseCaseResult<IngredientDetailModel>>;
