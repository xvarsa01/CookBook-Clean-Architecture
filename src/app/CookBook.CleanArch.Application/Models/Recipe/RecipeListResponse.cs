using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeListResponse
(
    RecipeId Id,
    RecipeName Name,
    ImageUrl? ImageUrl,
    RecipeType RecipeType
);
