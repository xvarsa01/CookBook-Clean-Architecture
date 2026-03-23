using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record DeleteRecipeUseCase(Guid Id) : IRequest<Result>;
