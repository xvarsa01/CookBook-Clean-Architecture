using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

namespace CookBook.CleanArch.Application.Commands.Recipes;

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
