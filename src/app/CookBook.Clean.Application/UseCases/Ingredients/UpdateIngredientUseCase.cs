using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Ingredients;

public record UpdateIngredientUseCase(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl) : IRequest<Result<Guid>>;
