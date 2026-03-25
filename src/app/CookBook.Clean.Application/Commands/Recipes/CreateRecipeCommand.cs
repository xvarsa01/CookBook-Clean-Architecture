using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record CreateRecipeCommand(RecipeCreateRequest Request) : ICommand<Guid>;

internal sealed class CreateRecipeCommandHandler(IRepository<Recipe, RecipeId> repository) : ICommandHandler<CreateRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var result = Recipe.Create(
            request.Request.Name,
            request.Request.Description,
            request.Request.ImageUrl,
            request.Request.Duration,
            request.Request.Type
        );
        
        if (result.IsFailure)
            return Result.Invalid<Guid>(result.Error);
        
        var createdRecipeId = await repository.InsertAsync(result.Value);
        return Result.Ok(createdRecipeId);
    }
}
