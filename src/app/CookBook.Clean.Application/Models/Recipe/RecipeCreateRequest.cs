using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

public record RecipeCreateRequest(
    RecipeName Name,
    string? Description,
    ImageUrl? ImageUrl,
    RecipeDuration Duration,
    RecipeType Type
);
