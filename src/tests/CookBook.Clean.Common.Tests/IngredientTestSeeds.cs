using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.TestsBase;

public class IngredientTestSeeds
{
    public static readonly IngredientBase Water =  IngredientBase.Create(name: "water",
        description: "water simply doesnt need description",
        imageUrl: ImageUrl.CreateObject("https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png").Value).Value;
    
    public static readonly IngredientBase Lemon =  IngredientBase.Create(name: "lemon",
        description: "bio eco raw from my garden",
        imageUrl: null).Value;
    
    public static readonly IngredientBase IngredientNotUsedInAnyRecipe =  IngredientBase.Create(name: "Newly Added",
        description: "this ingredient should not be in any recipe at start of test and should be adder later ",
        imageUrl: null).Value;
    
    public static readonly IngredientBase MinimalisticIngredient =  IngredientBase.Create(name: "minimalistic",
        description: null,
        imageUrl: null).Value;
    
    public static readonly IngredientBase IngredientForTestOfDelete =  IngredientBase.Create(name: "delete me",
        description: "i will be deleted",
        imageUrl: null).Value;
    
    public static readonly IngredientBase IngredientForTestOfUpdate =  IngredientBase.Create(name: "update me",
        description: "this will be updated",
        imageUrl: null).Value;
    
    public static List<IngredientBase> SeededIngredients() =>
    [
        Water,
        MinimalisticIngredient,
        IngredientForTestOfDelete,
        IngredientForTestOfUpdate,
    ];
}
