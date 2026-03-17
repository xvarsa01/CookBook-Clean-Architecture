using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.Recipe.GetList;

public record GetListRecipeUseCase() : IRequest<UseCaseResult<List<RecipeListModel>>>;
