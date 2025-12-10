using CookBook.Clean.Core.Ingredient;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.Get;

public record GetIngredientUseCase(Guid Id) : IRequest<UseCaseResult<GetIngredientResult>>;