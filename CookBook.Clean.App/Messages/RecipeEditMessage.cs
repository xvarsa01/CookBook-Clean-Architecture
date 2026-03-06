namespace CookBook.Clean.App.Messages;

public record RecipeEditMessage
{
    public required Guid RecipeId { get; init; }
}