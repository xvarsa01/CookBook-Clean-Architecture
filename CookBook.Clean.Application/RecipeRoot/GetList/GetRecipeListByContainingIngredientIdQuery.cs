using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.GetList;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IRequest<UseCaseResult<List<RecipeListModel>>>;
