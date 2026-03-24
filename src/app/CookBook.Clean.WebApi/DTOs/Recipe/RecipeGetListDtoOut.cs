
namespace CookBook.Clean.WebApi.DTOs.Recipe;

public record RecipeGetListDtoOut
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}
