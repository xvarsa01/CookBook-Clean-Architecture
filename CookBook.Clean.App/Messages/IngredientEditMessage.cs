namespace CookBook.Clean.App.Messages;

public record IngredientEditMessage
{
    public required Guid IngredientId { get; init; }
}