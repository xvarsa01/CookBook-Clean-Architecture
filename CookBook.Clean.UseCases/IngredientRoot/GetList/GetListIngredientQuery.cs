using CookBook.Clean.UseCases.Filters;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.GetList;

public record GetListIngredientQuery(IngredientFilter filter, PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<IngredientListModel>>>;
