using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Application.Models.Ingredient;

public record IngredientUpdateRequest(Guid Id, string? Name, string? Description, ImageUrl? ImageUrl);
