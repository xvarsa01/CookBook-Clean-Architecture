using CookBook.CleanArch.Domain.Ingredient.ValueObjects;

namespace CookBook.CleanArch.Presentation.MauiApplication.Messages;

public record IngredientEditMessage
{
    public required IngredientId IngredientId { get; init; }
}
