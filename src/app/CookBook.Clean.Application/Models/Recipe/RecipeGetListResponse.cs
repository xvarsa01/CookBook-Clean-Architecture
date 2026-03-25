using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

public record RecipeGetListResponse
(
    Guid Id,
    RecipeName Name,
    ImageUrl? ImageUrl
);
