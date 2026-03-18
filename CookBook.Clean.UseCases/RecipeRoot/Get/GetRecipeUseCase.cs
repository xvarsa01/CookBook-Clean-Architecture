using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.Get;

public record GetRecipeUseCase(Guid Id) : IRequest<UseCaseResult<RecipeDetailModel>>;
