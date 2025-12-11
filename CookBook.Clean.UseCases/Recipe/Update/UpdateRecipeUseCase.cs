using MediatR;

namespace CookBook.Clean.UseCases.Recipe.Update;

public record UpdateRecipeUseCase(Guid Id, string? NewName, string? NewDescription, string? NewImageUrl) : IRequest<UseCaseResult<UpdateRecipeResult>>
{
    
}
