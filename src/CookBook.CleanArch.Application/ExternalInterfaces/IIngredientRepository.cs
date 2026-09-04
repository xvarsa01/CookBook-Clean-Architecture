using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;

namespace CookBook.CleanArch.Application.ExternalInterfaces;

public interface IIngredientRepository : IRepository<Ingredient, IngredientId>
{
}
