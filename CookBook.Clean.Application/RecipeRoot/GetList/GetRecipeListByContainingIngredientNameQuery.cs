using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.GetList;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IRequest<UseCaseResult<List<RecipeListModel>>>;
