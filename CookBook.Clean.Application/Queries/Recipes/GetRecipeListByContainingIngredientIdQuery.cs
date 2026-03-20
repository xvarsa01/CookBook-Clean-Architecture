using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IRequest<UseCaseResult<List<RecipeListModel>>>;
