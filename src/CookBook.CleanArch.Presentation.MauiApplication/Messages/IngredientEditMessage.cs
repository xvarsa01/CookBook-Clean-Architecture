using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Presentation.MauiApplication.Messages;

public record IngredientEditMessage
{
    public required IngredientId IngredientId { get; init; }
}
