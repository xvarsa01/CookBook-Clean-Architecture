using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Domain;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.Errors;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Commands;

public record CreateRecipeCommand(RecipeCreateRequest Request) : ICommand<RecipeId>;

internal sealed class CreateRecipeCommandHandler(
    IRepository<Recipe, RecipeId> repository,
    IRepository<Ingredient, IngredientId> ingredientRepository) : ICommandHandler<CreateRecipeCommand,RecipeId>
{
    public async Task<Result<RecipeId>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        List<RecipeCreateIngredient> ingredients = [];
        foreach (var ingredientRequest in request.Request.Ingredients ?? [])
        {
            var ingredientExists = await ingredientRepository.GetByIdAsync(ingredientRequest.IngredientId);
            if (ingredientExists is null)
            {
                return Result.NotFound<RecipeId>(IngredientErrors.IngredientNotFoundError(ingredientRequest.IngredientId));
            }

            ingredients.Add(new RecipeCreateIngredient(
                ingredientRequest.IngredientId,
                ingredientRequest.Amount,
                ingredientRequest.Unit));
        }

        var result = Recipe.Create(
            request.Request.Name,
            request.Request.Description,
            request.Request.ImageUrl,
            request.Request.Duration,
            request.Request.Type,
            ingredients
        );
        
        if (result.IsFailure)
            return Result.Invalid<RecipeId>(result.Error);
        
        var createdRecipeId = repository.Add(result.Value);
        return Result.Ok(createdRecipeId);
    }
}
