using CookBook.CleanArch.Domain.Ingredient;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public class IngredientTestSeeds
{
    public static readonly Ingredient Water =  Ingredient.Create(name: "water",
        description: "water simply doesnt need description",
        imageUrl: ImageUrl.CreateObject("https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png").Value).Value;
    
    public static readonly Ingredient Lemon =  Ingredient.Create(name: "lemon",
        description: "bio eco raw from my garden",
        imageUrl: null).Value;
    
    public static readonly Ingredient IngredientNotUsedInAnyRecipe =  Ingredient.Create(name: "Newly Added",
        description: "this ingredient should not be in any recipe at start of test and should be adder later ",
        imageUrl: null).Value;
    
    public static readonly Ingredient MinimalisticIngredient =  Ingredient.Create(name: "minimalistic",
        description: null,
        imageUrl: null).Value;
    
    public static readonly Ingredient IngredientForTestOfDelete =  Ingredient.Create(name: "delete me",
        description: "i will be deleted",
        imageUrl: null).Value;
    
    public static readonly Ingredient IngredientForTestOfUpdate =  Ingredient.Create(name: "update me",
        description: "this will be updated",
        imageUrl: null).Value;
    
    public static List<Ingredient> SeededIngredients() =>
    [
        Water,
        MinimalisticIngredient,
        IngredientForTestOfDelete,
        IngredientForTestOfUpdate,
    ];
}
