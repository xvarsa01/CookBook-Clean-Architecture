using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public static class RecipeTestSeeds
{
    private static IReadOnlyCollection<RecipeCreateIngredient> SingleIngredient() =>
    [
        new RecipeCreateIngredient(
            IngredientTestSeeds.Water.Id,
            IngredientAmount.CreateObject(100).Value,
            MeasurementUnit.Ml)
    ];

    public static Recipe EmptyRecipe()
    {
        return Recipe.Create(name: RecipeName.CreateObject("empty recipe").Value,
            description: "baseline recipe",
            imageUrl: ImageUrl.CreateObject("http://a.png").Value,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: SingleIngredient()).Value;
    }

    public static Recipe MinimalisticRecipe()
    {
        return Recipe.Create(name: RecipeName.CreateObject("minimalistic").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: SingleIngredient()).Value;
    }

    public static Recipe RecipeWithSingleIngredient()
    {
        return Recipe.Create(name: RecipeName.CreateObject("recipe with 1 ingredient").Value,
            description: "this will be added",
            imageUrl: ImageUrl.CreateObject("http://a.png").Value,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients:
            [
                new RecipeCreateIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Ml)
            ]).Value;
    }

    public static Recipe RecipeWithMultipleIngredients()
    {
        return Recipe.Create(name: RecipeName.CreateObject("recipe with multiple ingredient").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients:
            [
                new RecipeCreateIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Ml),
                new RecipeCreateIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(1).Value,
                    MeasurementUnit.None)
            ]).Value;
    }
    
    public static Recipe RecipeWithDuplicateIngredientEntries()
    {
        return Recipe.Create(name: RecipeName.CreateObject("recipe with lemon used twice").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients:
            [
                new RecipeCreateIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(500).Value,
                    MeasurementUnit.Ml),
                new RecipeCreateIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Ml),
                new RecipeCreateIngredient(IngredientTestSeeds.Lemon.Id, IngredientAmount.CreateObject(1).Value,
                    MeasurementUnit.Pieces)
            ]).Value;
    }
    
    public static Recipe RecipeFullWith10Ingredients()
    {
        return Recipe.Create(name: RecipeName.CreateObject("recipe with 10 ingredient").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: Enumerable.Range(0, 10)
                .Select(_ => new RecipeCreateIngredient(
                    IngredientTestSeeds.Water.Id,
                    IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Ml))
                .ToList()).Value;
    }

    public static Recipe RecipeForTestOfDeleteWithoutIngredient()
    {
        return Recipe.Create(name: RecipeName.CreateObject("delete me").Value,
            description: "i will be deleted simply",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: SingleIngredient()).Value;
    }

    public static Recipe RecipeForTestOfDeleteWithIngredient()
    {
        return Recipe.Create(name: RecipeName.CreateObject("delete me").Value,
            description: "i will be deleted, but my ingredients should remain in DB",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients:
            [
                new RecipeCreateIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Ml)
            ]).Value;
    }
    
    public static Recipe RecipeForTestOfUpdate(){
        return Recipe.Create(name: RecipeName.CreateObject("update me").Value,
            description: "this will be updated",
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients:
            [
                new RecipeCreateIngredient(IngredientTestSeeds.Water.Id, IngredientAmount.CreateObject(100).Value,
                    MeasurementUnit.Ml)
            ]).Value;
    }
    
    public static Recipe RecipeForTestOfSearch1(){
        return Recipe.Create(name: RecipeName.CreateObject("abcd").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: SingleIngredient()).Value;
    }
    
    public static Recipe RecipeForTestOfSearch2(){
        return Recipe.Create(name: RecipeName.CreateObject("ABCD").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: SingleIngredient()).Value;
    }
    
    public static Recipe RecipeForTestOfSearch3(){
        return Recipe.Create(name: RecipeName.CreateObject("BCD EFGH").Value,
            description: null,
            imageUrl: null,
            duration: RecipeDuration.CreateObject(TimeSpan.FromMinutes(10)).Value,
            type: RecipeType.Other,
            ingredients: SingleIngredient()).Value;
    }
    
    public static List<Recipe> SeededRecipes() =>
    [
        EmptyRecipe(),
        MinimalisticRecipe(),
        RecipeWithSingleIngredient(),
        RecipeWithMultipleIngredients(),
        RecipeForTestOfDeleteWithoutIngredient(),
        RecipeForTestOfDeleteWithIngredient(),
        RecipeForTestOfUpdate(),
        RecipeForTestOfSearch1(),
        RecipeForTestOfSearch2(),
        RecipeForTestOfSearch3()
    ];
}
