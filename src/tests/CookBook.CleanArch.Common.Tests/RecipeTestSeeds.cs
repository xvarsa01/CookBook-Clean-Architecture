using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.Enums;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public static class RecipeTestSeeds
{
    public static Recipe EmptyRecipe()
    {
        return Recipe.Create(name: RecipeName.CreateObject("empty recipe").Value,
            description: "no ingredients",
            imageUrl: ImageUrl.CreateObject("http://a.png").Value,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
    }

    public static Recipe MinimalisticRecipe()
    {
        return Recipe.Create(name: RecipeName.CreateObject("minimalistic").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
    }

    public static Recipe RecipeWithSingleIngredient()
    {
        var recipe = Recipe.Create(name: RecipeName.CreateObject("recipe with 1 ingredient").Value,
            description: "this will be added",
            imageUrl: ImageUrl.CreateObject("http://a.png").Value,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        return recipe;
    }

    public static Recipe RecipeWithMultipleIngredients()
    {
        var recipe = Recipe.Create(name: RecipeName.CreateObject("recipe with multiple ingredient").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(1).Value, MeasurementUnit.Unit);
        return recipe;
    }
    
    public static Recipe RecipeWithDuplicateIngredientEntries()
    {
        var recipe = Recipe.Create(name: RecipeName.CreateObject("recipe with lemon used twice").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(500).Value, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        recipe.AddIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(1).Value, MeasurementUnit.Unit);
        return recipe;
    }
    
    public static Recipe RecipeFullWith10Ingredients()
    {
        var recipe = Recipe.Create(name: RecipeName.CreateObject("recipe with 10 ingredient").Value,
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

    public static Recipe RecipeForTestOfDeleteWithoutIngredient()
    {
        return Recipe.Create(name: RecipeName.CreateObject("delete me").Value,
            description: "i will be deleted simply, because i dont contain any ingredients",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
    }

    public static Recipe RecipeForTestOfDeleteWithIngredient()
    {
        var recipe = Recipe.Create(name: RecipeName.CreateObject("delete me").Value,
            description: "i will be deleted, but my ingredients should remain in DB",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;
        
        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        return recipe;
    }
    
    public static Recipe RecipeForTestOfUpdate(){
        var recipe = Recipe.Create(name: RecipeName.CreateObject("update me").Value,
            description: "this will be updated",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.None).Value;

        recipe.AddIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value, MeasurementUnit.Ml);
        return recipe;
    }
    
    public static List<Recipe> SeededRecipes() =>
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
