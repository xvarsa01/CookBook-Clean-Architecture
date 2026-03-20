using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.IngredientRoot.GetList;

public record GetListIngredientQuery(IngredientFilter filter, PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<IngredientListModel>>>;
