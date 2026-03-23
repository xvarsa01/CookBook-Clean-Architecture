using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListQuery(RecipeFilter Filter, PagingOptions? PagingOptions = null) : IRequest<Result<List<RecipeListModel>>>;
