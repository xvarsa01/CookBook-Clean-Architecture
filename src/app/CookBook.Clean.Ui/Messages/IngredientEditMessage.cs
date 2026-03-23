namespace CookBook.Clean.Ui.Messages;

public record IngredientEditMessage
{
    public required Guid IngredientId { get; init; }
}