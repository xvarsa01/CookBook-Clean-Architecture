using CookBook.CleanArch.Domain.RecipeRoot.Enums;
using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeCreateRequest(
    RecipeName Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration Duration,
    RecipeType Type
);
