using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public record GetRecipeListByContainingIngredientNameQuery(string IngredientNameSubstring) : IRequest<UseCaseResult<List<RecipeListModel>>>;
