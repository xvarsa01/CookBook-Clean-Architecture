using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientListQuery(IngredientFilter Filter, PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<IngredientListModel>>>;
