using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.IngredientRoot.ValueObjects;

public record IngredientId(Guid Id) : StronglyTypedId(Id);
