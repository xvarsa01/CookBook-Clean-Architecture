using CookBook.Clean.Core.Recipe;

namespace CookBook.Clean.WebApi.DTOs;

public class RecipeRemoveIngredientRequestDto
{
    public required Guid IngredientId { get; set; }
}
