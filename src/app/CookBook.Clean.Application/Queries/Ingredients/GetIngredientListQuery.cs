using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.Queries.Ingredients;

public record GetIngredientListQuery(IngredientFilter Filter, PagingOptions? PagingOptions = null) : IRequest<Result<List<IngredientListModel>>>;
