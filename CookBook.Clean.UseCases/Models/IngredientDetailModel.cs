namespace CookBook.Clean.UseCases.Models;

public class IngredientDetailModel : ModelBase
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public static IngredientDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Description = string.Empty,
        ImageUrl =  string.Empty,
    };
}
