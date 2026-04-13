using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeUpdateRequest(
    RecipeId Id,
    RecipeName? Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration? Duration,
    RecipeType? Type
);
