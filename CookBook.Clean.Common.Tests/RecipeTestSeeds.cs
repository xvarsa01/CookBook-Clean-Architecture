using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;

namespace CookBook.Clean.TestsBase;

public static class RecipeTestSeeds
{
    public static RecipeEntity EmptyRecipe()
    {
        return new RecipeEntity(
            name: new RecipeName("empty recipe"),
            description: "no ingredients",
            imageUrl: "no image",
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
    }

    public static RecipeEntity MinimalisticRecipe()
    {
        return new RecipeEntity(
            name: new RecipeName("minimalistic"),
            description: null,
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
    }

    public static RecipeEntity RecipeWithSingleIngredient()
    {
        var recipe = new RecipeEntity(
            name: new RecipeName("recipe with 1 ingredient"),
            description: "this will be added",
            imageUrl: "no image",
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, new IngredientAmount(100), MeasurementUnit.Ml);
        return recipe;
    }

    public static RecipeEntity RecipeWithMultipleIngredients()
    {
        var recipe = new RecipeEntity(
            name: new RecipeName("recipe with multiple ingredient"),
            description: null,
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, new IngredientAmount(100), MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, new IngredientAmount(1), MeasurementUnit.Unit);
        return recipe;
    }
    
    public static RecipeEntity RecipeWithDuplicateIngredientEntries()
    {
        var recipe = new RecipeEntity(
            name: new RecipeName("recipe with lemon used twice"),
            description: null,
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, new IngredientAmount(500), MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, new IngredientAmount(100), MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, new IngredientAmount(1), MeasurementUnit.Unit);
        return recipe;
    }
    
    public static RecipeEntity RecipeFullWith10Ingredients()
    {
        var recipe = new RecipeEntity(
            name: new RecipeName("recipe with 10 ingredient"),
            description: null,
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
        
        for (int i = 0; i < 10; i++)
        {
            recipe.AddIngredient(IngredientTestSeeds.Water.Id, new IngredientAmount(100), MeasurementUnit.Ml);
        }
        return recipe;
    }

    public static RecipeEntity RecipeForTestOfDeleteWithoutIngredient()
    {
        return new RecipeEntity(
            name: new RecipeName("delete me"),
            description: "i will be deleted simply, because i dont contain any ingredients",
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
    }

    public static RecipeEntity RecipeForTestOfDeleteWithIngredient()
    {
        var recipe = new RecipeEntity(
            name: new RecipeName("delete me"),
            description: "i will be deleted, but my ingredients should remain in DB",
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, new IngredientAmount(100), MeasurementUnit.Ml);
        return recipe;
    }
    
    public static RecipeEntity RecipeForTestOfUpdate(){
        var recipe = new  RecipeEntity(
            name: new RecipeName("update me"),
            description: "this will be updated",
            imageUrl: null,
            duration: new RecipeDuration(TimeSpan.FromMinutes(10)),
            type: RecipeType.None);

        recipe.AddIngredient(IngredientTestSeeds.Water.Id, new IngredientAmount(100), MeasurementUnit.Ml);
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
