using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.Get;

public record GetRecipeQuery(Guid Id) : IRequest<UseCaseResult<RecipeDetailModel>>;
