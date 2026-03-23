using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.TestsBase;

public static class RecipeTestSeeds
{
    public static RecipeEntity EmptyRecipe()
    {
        return RecipeEntity.Create(name: RecipeName.CreateObject("empty recipe").Value,
            description: "no ingredients",
            imageUrl: ImageUrl.CreateObject("http://a.png").Value,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
    }

    public static RecipeEntity MinimalisticRecipe()
    {
        return RecipeEntity.Create(name: RecipeName.CreateObject("minimalistic").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
    }

    public static RecipeEntity RecipeWithSingleIngredient()
    {
        var recipe = RecipeEntity.Create(name: RecipeName.CreateObject("recipe with 1 ingredient").Value,
            description: "this will be added",
            imageUrl: ImageUrl.CreateObject("http://a.png").Value,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        return recipe;
    }

    public static RecipeEntity RecipeWithMultipleIngredients()
    {
        var recipe = RecipeEntity.Create(name: RecipeName.CreateObject("recipe with multiple ingredient").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(1).Value, MeasurementUnit.Unit);
        return recipe;
    }
    
    public static RecipeEntity RecipeWithDuplicateIngredientEntries()
    {
        var recipe = RecipeEntity.Create(name: RecipeName.CreateObject("recipe with lemon used twice").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(1).Value, MeasurementUnit.Unit);
        return recipe;
    }
    
    public static RecipeEntity RecipeFullWith10Ingredients()
    {
        var recipe = RecipeEntity.Create(name: RecipeName.CreateObject("recipe with 10 ingredient").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        for (int i = 0; i < 10; i++)
        {
            recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        }
        return recipe;
    }

    public static RecipeEntity RecipeForTestOfDeleteWithoutIngredient()
    {
        return RecipeEntity.Create(name: RecipeName.CreateObject("delete me").Value,
            description: "i will be deleted simply, because i dont contain any ingredients",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
    }

    public static RecipeEntity RecipeForTestOfDeleteWithIngredient()
    {
        var recipe = RecipeEntity.Create(name: RecipeName.CreateObject("delete me").Value,
            description: "i will be deleted, but my ingredients should remain in DB",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        return recipe;
    }
    
    public static RecipeEntity RecipeForTestOfUpdate(){
        var recipe = RecipeEntity.Create(name: RecipeName.CreateObject("update me").Value,
            description: "this will be updated",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;

        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        return recipe;
    }
    
    public static List<RecipeEntity> SeededRecipes() =>
    [
        EmptyRecipe(),
        MinimalisticRecipe(),
        RecipeWithSingleIngredient(),
        RecipeWithMultipleIngredients(),
        RecipeForTestOfDeleteWithoutIngredient(),
        RecipeForTestOfDeleteWithIngredient(),
        RecipeForTestOfUpdate()
    ];
}
