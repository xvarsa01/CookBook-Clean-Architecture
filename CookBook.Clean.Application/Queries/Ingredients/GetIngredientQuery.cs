using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientQuery(Guid Id) : IRequest<UseCaseResult<IngredientDetailModel>>;
