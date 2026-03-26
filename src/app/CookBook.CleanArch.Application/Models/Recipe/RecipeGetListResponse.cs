using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeGetListResponse
(
    RecipeId Id,
    RecipeName Name,
    ImageUrl? ImageUrl
);
