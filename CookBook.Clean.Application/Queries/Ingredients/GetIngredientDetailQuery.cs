using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientDetailQuery(Guid Id) : IRequest<UseCaseResult<IngredientDetailModel>>;
