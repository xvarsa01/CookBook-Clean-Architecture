using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public record GetListRecipeQuery(PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<RecipeListModel>>>;
