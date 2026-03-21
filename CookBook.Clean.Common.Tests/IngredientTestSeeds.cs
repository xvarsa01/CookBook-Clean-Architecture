using CookBook.Clean.Core.IngredientRoot;

namespace CookBook.Clean.TestsBase;

public class IngredientTestSeeds
{
    public static readonly IngredientEntity Water =  new(
        name: "water",
        description: "water simply doesnt need description",
        imageUrl: "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png");
    
    public static readonly IngredientEntity Lemon =  new(
        name: "lemon",
        description: "bio eco raw from my garden",
        imageUrl: null);
    
    public static readonly IngredientEntity IngredientNotUsedInAnyRecipe =  new(
        name: "Newly Added",
        description: "this ingredient should not be in any recipe at start of test and should be adder later ",
        imageUrl: null);
    
    public static readonly IngredientEntity MinimalisticIngredient =  new(
        name: "minimalistic",
        description: null,
        imageUrl: null);
    
    public static readonly IngredientEntity IngredientForTestOfDelete =  new(
        name: "delete me",
        description: "i will be deleted",
        imageUrl: null);
    
    public static readonly IngredientEntity IngredientForTestOfUpdate =  new(
        name: "update me",
        description: "this will be updated",
        imageUrl: null);
    
    public static List<IngredientEntity> SeededIngredients() =>
    [
        Water,
        MinimalisticIngredient,
        IngredientForTestOfDelete,
        IngredientForTestOfUpdate,
    ];
}
