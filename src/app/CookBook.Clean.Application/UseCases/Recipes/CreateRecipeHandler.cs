using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Core;
using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using MediatR;

namespace CookBook.Clean.Application.UseCases.Recipes;

public record CreateRecipeUseCase(string Name, string? Description, string? ImageUrl, TimeSpan Duration, RecipeType RecipeType) : IRequest<Result<Guid>>;

internal class CreateRecipeHandler(IRepository<RecipeEntity> repository, IRecipeMapper mapper) : IRequestHandler<CreateRecipeUseCase, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateRecipeUseCase request, CancellationToken cancellationToken)
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
