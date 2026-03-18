using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.RecipeRoot.GetList;

public record GetListRecipeUseCase() : IRequest<UseCaseResult<List<RecipeListModel>>>;
