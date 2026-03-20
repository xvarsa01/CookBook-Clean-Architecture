namespace CookBook.Clean.Ui.Messages;

public record RecipeEditMessage
{
    public required Guid RecipeId { get; init; }
}