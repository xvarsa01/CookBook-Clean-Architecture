using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.Common.Tests;

public static class IngredientTestSeeds
{
    private const string ValidImageUrl = "https://i.imgur.com/YYPzexp.png";
    
    
    public static readonly Ingredient Water = Ingredient.Create(
        name: "water",
        description: "water simply doesnt need description",
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient Lemon = Ingredient.Create(
        name: "lemon",
        description: "bio eco raw from my garden",
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient UsedInSingleRecipe = Ingredient.Create(
        name: "used in single recipe",
        description: null,
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient IngredientNotUsedInAnyRecipe =  Ingredient.Create(
        name: "Newly Added",
        description: "this ingredient should not be in any recipe at start of test and should be adder later ",
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient MinimalisticIngredient = Ingredient.Create(
        name: "minimalistic",
        description: null,
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient IngredientForTestOfDelete = Ingredient.Create(
        name: "delete me",
        description: "i will be deleted",
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient IngredientForTestOfUpdate = Ingredient.Create(
        name: "update me",
        description: "this will be updated",
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient IngredientForNamingTest1 = Ingredient.Create(
        name: "AXA",
        description: null,
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    public static readonly Ingredient IngredientForNamingTest2 = Ingredient.Create(
        name: "AxA",
        description: null,
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    public static readonly Ingredient IngredientForNamingTest3 = Ingredient.Create(
        name: "AXXXA",
        description: null,
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    public static readonly Ingredient IngredientForNamingTest4 = Ingredient.Create(
        name: "X X",
        description: null,
        imageUrl: ImageUrl.CreateObject(ValidImageUrl).Value).Value;
    
    public static readonly Ingredient IngredientWithNullImage1 = Ingredient.Create(
        name: "Null image 1",
        description: null,
        imageUrl: null).Value;
    
    public static readonly Ingredient IngredientWithNullImage2 = Ingredient.Create(
        name: "Null image 2",
        description: null,
        imageUrl: null).Value;
    
    
    public static List<Ingredient> SeededIngredients =>
    [
        Water,
        Lemon,
        UsedInSingleRecipe,
        IngredientNotUsedInAnyRecipe,
        MinimalisticIngredient,
        IngredientForTestOfDelete,
        IngredientForTestOfUpdate,
        IngredientForNamingTest1,
        IngredientForNamingTest2,
        IngredientForNamingTest3,
        IngredientForNamingTest4,
        IngredientWithNullImage1,
        IngredientWithNullImage2
    ];
}
