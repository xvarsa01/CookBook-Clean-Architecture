using CookBook.Clean.UseCases.Filters;
using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public record GetListRecipeQuery(RecipeFilter filter, PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<RecipeListModel>>>;
