namespace CookBook.CleanArch.Presentation.MauiApplication.Messages;

public record RecipeEditMessage
{
    public required Guid RecipeId { get; init; }
}
