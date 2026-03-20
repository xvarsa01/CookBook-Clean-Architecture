using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.Queries.Recipes;

public record GetRecipeDetailQuery(Guid Id) : IRequest<UseCaseResult<RecipeDetailModel>>;
