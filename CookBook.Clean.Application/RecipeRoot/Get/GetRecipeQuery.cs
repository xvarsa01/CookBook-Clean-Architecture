using CookBook.Clean.Application.Models;
using MediatR;

namespace CookBook.Clean.Application.RecipeRoot.Get;

public record GetRecipeQuery(Guid Id) : IRequest<UseCaseResult<RecipeDetailModel>>;
