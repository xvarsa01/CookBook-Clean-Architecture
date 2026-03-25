using CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Recipe;

public record RecipeGetListResponse
(
    Guid Id,
    RecipeName Name,
    ImageUrl? ImageUrl
);
