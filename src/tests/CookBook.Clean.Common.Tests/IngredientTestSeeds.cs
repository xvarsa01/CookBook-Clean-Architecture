using CookBook.Clean.Core.IngredientRoot;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.TestsBase;

public class IngredientTestSeeds
{
    public static readonly IngredientEntity Water =  IngredientEntity.Create(name: "water",
        description: "water simply doesnt need description",
        imageUrl: ImageUrl.CreateObject("https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png").Value).Value;
    
    public static readonly IngredientEntity Lemon =  IngredientEntity.Create(name: "lemon",
        description: "bio eco raw from my garden",
        imageUrl: null).Value;
    
    public static readonly IngredientEntity IngredientNotUsedInAnyRecipe =  IngredientEntity.Create(name: "Newly Added",
        description: "this ingredient should not be in any recipe at start of test and should be adder later ",
        imageUrl: null).Value;
    
    public static readonly IngredientEntity MinimalisticIngredient =  IngredientEntity.Create(name: "minimalistic",
        description: null,
        imageUrl: null).Value;
    
    public static readonly IngredientEntity IngredientForTestOfDelete =  IngredientEntity.Create(name: "delete me",
        description: "i will be deleted",
        imageUrl: null).Value;
    
    public static readonly IngredientEntity IngredientForTestOfUpdate =  IngredientEntity.Create(name: "update me",
        description: "this will be updated",
        imageUrl: null).Value;
    
    public static List<IngredientEntity> SeededIngredients() =>
    [
        Water,
        MinimalisticIngredient,
        IngredientForTestOfDelete,
        IngredientForTestOfUpdate,
    ];
}
