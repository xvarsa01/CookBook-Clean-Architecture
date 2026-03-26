using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.Domain.Recipe.ValueObjects;

public record IngredientInRecipeId(Guid Id) : StronglyTypedId(Id), IValueObject<Guid>, IValueObjectFactory<IngredientInRecipeId, Guid>
{
    public Guid Value => Id;
    
    public static Result<IngredientInRecipeId> CreateObject()
    {
        return Result.Ok(new IngredientInRecipeId((Guid.NewGuid())));
    }
    public static Result<IngredientInRecipeId> CreateObject(Guid id)
    {
        return Result.Ok(new IngredientInRecipeId(id));
    }
}

