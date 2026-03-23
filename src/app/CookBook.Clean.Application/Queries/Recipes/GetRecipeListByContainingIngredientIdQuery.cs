using CookBook.Clean.Application.Models;
using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeListByContainingIngredientIdQuery(Guid IngredientId) : IRequest<Result<List<RecipeListModel>>>;
