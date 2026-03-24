using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.Application.Commands.Recipes;

public record CreateRecipeCommand(string Name, string? Description, string? ImageUrl, TimeSpan Duration, RecipeType RecipeType) : ICommand<Guid>;

internal sealed class CreateRecipeCommandHandler(IRepository<Recipe> repository, IRecipeMapper mapper) : ICommandHandler<CreateRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newRecipe = mapper.MapToEntity(request);
            var createdItemId = await repository.InsertAsync(newRecipe);
            return Result.Ok(createdItemId);
        }
        catch (Exception e)
        {
            return Result.Invalid<Guid>(e.Message);
        }
    }
}
