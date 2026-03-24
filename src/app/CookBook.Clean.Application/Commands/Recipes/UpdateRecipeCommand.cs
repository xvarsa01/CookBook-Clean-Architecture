using CookBook.Clean.Application.Abstraction;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Models.Recipe;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Commands.Recipes;

public record UpdateRecipeCommand(RecipeUpdateDto Dto) : ICommand<Guid>;

internal sealed class UpdateRecipeCommandHandler(IRepository<Recipe> repository) : ICommandHandler<UpdateRecipeCommand,Guid>
{
    public async Task<Result<Guid>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Dto.Id);
        if (existing is null)
        {
            return Result.NotFound<Guid>("Recipe not found");
        }

        if (request.Dto.Name is not null)
        {
            existing.UpdateName(request.Dto.Name);
        }
        
        if (request.Dto.Description is not null)
        {
            existing.UpdateDescription(request.Dto.Description);
        }
        
        existing.UpdateRest(request.Dto.ImageUrl, request.Dto.Duration, request.Dto.Type);

        var id = await repository.UpdateAsync(existing);
        if (id is null)
        {
            return Result.Invalid<Guid>("Update failed");
        }
        
        return Result.Ok(id.Value);
    }
}
