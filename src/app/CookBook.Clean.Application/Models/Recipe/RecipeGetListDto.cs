using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.Application.Models.Recipe;

public record RecipeGetListDto
{
    public required Guid Id { get; set; }
    public required RecipeName Name { get; set; }
    public ImageUrl? ImageUrl { get; set; }
}
