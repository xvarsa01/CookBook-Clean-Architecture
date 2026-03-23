using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IRequest<Result<List<RecipeListModel>>>;
