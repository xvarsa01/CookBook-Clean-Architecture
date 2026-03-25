using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.RecipeRoot.ValueObjects;

public record RecipeId(Guid Id) : StronglyTypedId(Id);

