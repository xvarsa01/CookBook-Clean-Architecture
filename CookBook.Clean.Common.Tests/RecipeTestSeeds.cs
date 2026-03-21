using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.TestsBase;

public static class RecipeTestSeeds
{
    public static RecipeEntity EmptyRecipe()
    {
        return new RecipeEntity(
            name: "empty recipe",
            description: "no ingredients",
            imageUrl: "no image",
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
    }

    public static RecipeEntity MinimalisticRecipe()
    {
        return new RecipeEntity(
            name: "minimalistic",
            description: null,
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
    }

    public static RecipeEntity RecipeWithSingleIngredient()
    {
        var recipe = new RecipeEntity(
            name: "recipe with 1 ingredient",
            description: "this will be added",
            imageUrl: "no image",
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, 100, MeasurementUnit.Ml);
        return recipe;
    }

    public static RecipeEntity RecipeWithMultipleIngredients()
    {
        var recipe = new RecipeEntity(
            name: "recipe with multiple ingredient",
            description: null,
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, 100, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, 1, MeasurementUnit.Unit);
        return recipe;
    }
    
    public static RecipeEntity RecipeWithDuplicateIngredientEntries()
    {
        var recipe = new RecipeEntity(
            name: "recipe with lemon used twice",
            description: null,
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, 500, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, 100, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, 1, MeasurementUnit.Unit);
        return recipe;
    }
    
    public static RecipeEntity RecipeFullWith10Ingredients()
    {
        var recipe = new RecipeEntity(
            name: "recipe with 10 ingredient",
            description: null,
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
        
        for (int i = 0; i < 10; i++)
        {
            recipe.AddIngredient(IngredientTestSeeds.Water.Id, 100, MeasurementUnit.Ml);
        }
        return recipe;
    }

    public static RecipeEntity RecipeForTestOfDeleteWithoutIngredient()
    {
        return new RecipeEntity(
            name: "delete me",
            description: "i will be deleted simply, because i dont contain any ingredients",
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
    }

    public static RecipeEntity RecipeForTestOfDeleteWithIngredient()
    {
        var recipe = new RecipeEntity(
            name: "delete me",
            description: "i will be deleted, but my ingredients should remain in DB",
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, 100, MeasurementUnit.Ml);
        return recipe;
    }
    
    public static RecipeEntity RecipeForTestOfUpdate(){
        var recipe = new  RecipeEntity(
            name: "update me",
            description: "this will be updated",
            imageUrl: null,
            duration: TimeSpan.FromMinutes(10),
            type: RecipeType.None);

        recipe.AddIngredient(IngredientTestSeeds.Water.Id, 100, MeasurementUnit.Ml);
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
