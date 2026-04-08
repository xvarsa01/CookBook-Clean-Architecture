using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;

namespace CookBook.CleanArch.Application.Commands.Recipes;

public record CreateRecipeCommand(RecipeCreateRequest Request) : ICommand<RecipeId>;

internal sealed class CreateRecipeCommandHandler(IRepository<Recipe, RecipeId> repository) : ICommandHandler<CreateRecipeCommand,RecipeId>
{
    // TODO unit of work on all commands
    public async Task<Result<RecipeId>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var result = Recipe.Create(
            request.Request.Name,
            request.Request.Description,
            request.Request.ImageUrl,
            request.Request.Duration,
            request.Request.Type
        );
        
        if (result.IsFailure)
            return Result.Invalid<RecipeId>(result.Error);
        
        var createdRecipeId = await repository.InsertAsync(result.Value);
        return Result.Ok(createdRecipeId);
    }
}
