using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;

public record DeleteIngredientUseCase(Guid Id) : IRequest<Result>;
