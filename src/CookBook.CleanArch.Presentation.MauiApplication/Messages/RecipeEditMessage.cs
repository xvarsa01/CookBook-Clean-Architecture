using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Presentation.MauiApplication.Messages;

public record RecipeEditMessage
{
    public required RecipeId RecipeId { get; init; }
}
