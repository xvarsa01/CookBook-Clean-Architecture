using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record DeleteRecipeUseCase(Guid Id) : IRequest<UseCaseResult>;
