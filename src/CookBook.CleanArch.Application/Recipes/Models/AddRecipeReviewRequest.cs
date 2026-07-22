namespace CookBook.CleanArch.Application.Recipes.Models;

public record AddRecipeReviewRequest(
    int Rating,
    string Comment
);
