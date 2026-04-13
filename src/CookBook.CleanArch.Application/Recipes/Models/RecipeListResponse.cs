using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Recipes.Models;

public record RecipeListResponse
(
    RecipeId Id,
    RecipeName Name,
    ImageUrl? ImageUrl,
    RecipeType RecipeType
);
