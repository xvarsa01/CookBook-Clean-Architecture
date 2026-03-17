using CookBook.Clean.UseCases.Models;
using MediatR;

namespace CookBook.Clean.UseCases.Ingredient.GetList;

public record GetListIngredientUseCase() : IRequest<UseCaseResult<List<IngredientListModel>>>;
