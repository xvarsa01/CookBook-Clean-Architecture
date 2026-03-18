using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.Get;

public record GetIngredientQuery(Guid Id) : IRequest<UseCaseResult<IngredientDetailModel>>;
