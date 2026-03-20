using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IRequest<UseCaseResult<List<RecipeListModel>>>;
