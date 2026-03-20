using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.IngredientRoot.Get;

public record GetIngredientQuery(Guid Id) : IRequest<UseCaseResult<IngredientDetailModel>>;
