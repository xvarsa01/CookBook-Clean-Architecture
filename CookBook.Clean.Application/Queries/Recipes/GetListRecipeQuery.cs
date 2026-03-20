using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetListRecipeQuery(RecipeFilter filter, PagingOptions? PagingOptions = null) : IRequest<UseCaseResult<List<RecipeListModel>>>;
