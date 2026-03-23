using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientDetailQuery(Guid Id) : IRequest<Result<IngredientDetailModel>>;
