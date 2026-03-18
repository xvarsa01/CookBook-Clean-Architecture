using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.GetList;

public record GetListIngredientQuery(PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<IngredientListModel>>>;
