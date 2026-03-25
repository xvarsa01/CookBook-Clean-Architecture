namespace CookBook.CleanArch.Presentation.MauiApplication.Messages;

public record IngredientEditMessage
{
    public required Guid IngredientId { get; init; }
}
