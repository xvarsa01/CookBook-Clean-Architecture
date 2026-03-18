using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.IngredientRoot.GetList;

public record GetListIngredientUseCase() : IRequest<UseCaseResult<List<IngredientListModel>>>;
