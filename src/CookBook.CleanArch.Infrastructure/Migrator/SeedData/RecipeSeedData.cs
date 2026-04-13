using CookBook.CleanArch.Domain.Recipes.Enums;

namespace CookBook.CleanArch.Infrastructure.Migrator.SeedData;

internal static class RecipeSeedData
{
    public static IReadOnlyList<RecipeSeedItem> Items =>
    [
        new RecipeSeedItem(
            name: "Mojito",
            description: "fresh mint cocktail",
            imageUrl: "https://images.unsplash.com/photo-1659046842567-2787b5c9c2fe?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Lime", amount: 1.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Mint", amount: 5.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 10.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Rum", amount: 50.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Soda", amount: 200.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Lemonade",
            description: "simple refreshing drink",
            imageUrl: null,
            duration: TimeSpan.Parse("00:03:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Lemon", amount: 2.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 20.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Water", amount: 500.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Orange Drink",
            description: null,
            imageUrl: null,
            duration: TimeSpan.Parse("00:02:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Orange Juice", amount: 250.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 5.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Latte",
            description: "coffee with steamed milk",
            imageUrl: null,
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)5,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Coffee", amount: 200.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 100.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Chocolate Milkshake",
            description: "sweet cold dessert drink",
            imageUrl: "https://images.unsplash.com/photo-1571328003758-4a3921661729?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:06:00"),
            type: (RecipeType)3,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 300.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Chocolate", amount: 50.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Banana", amount: 1.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Strawberry Smoothie",
            description: null,
            imageUrl: "https://images.unsplash.com/photo-1553530666-ba11a7da3888?q=80&w=686&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Strawberry", amount: 5.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 200.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 10.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Cappuccino",
            description: "espresso coffee with milk foam",
            imageUrl: "https://images.unsplash.com/photo-1594261956806-3ad03785c9b4?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)5,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Espresso Shot", amount: 2.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 150.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 8.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Cinnamon", amount: 1.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Caramel Macchiato",
            description: "espresso with milk and caramel",
            imageUrl: "https://images.unsplash.com/photo-1517701550927-30cf4ba1dba5?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)5,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Espresso Shot", amount: 2.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 180.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Caramel Syrup", amount: 20.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Vanilla Syrup", amount: 10.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Iced Americano",
            description: "cold espresso diluted with water",
            imageUrl: "https://images.unsplash.com/photo-1627898784518-b6ee18b7f22a?q=80&w=1026&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:03:00"),
            type: (RecipeType)5,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Espresso Shot", amount: 2.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Water", amount: 200.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 6.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Matcha Latte",
            description: "creamy green tea latte",
            imageUrl: "https://images.unsplash.com/photo-1515823064-d6e0c04616a7?q=80&w=1171&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)5,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Green Tea", amount: 6.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 220.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 10.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Vanilla Syrup", amount: 10.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Chai Latte",
            description: "spiced black tea with milk",
            imageUrl: "https://images.unsplash.com/photo-1561336526-2914f13ceb36?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Black Tea", amount: 180.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 150.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Honey", amount: 12.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Cinnamon", amount: 2.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Gin Tonic",
            description: "classic gin and tonic cocktail",
            imageUrl: "https://images.unsplash.com/photo-1675370734208-120b43d3c7ac?q=80&w=686&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:03:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Gin", amount: 50.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Tonic Water", amount: 150.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Lime", amount: 1.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 5.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Margarita",
            description: "tequila citrus cocktail",
            imageUrl: "https://images.unsplash.com/photo-1655546837806-76a6dd54ee2b?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Tequila", amount: 50.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Triple Sec", amount: 20.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Lemon", amount: 1.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 5.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Cosmopolitan",
            description: "vodka cocktail with cranberry and citrus",
            imageUrl: "https://images.unsplash.com/photo-1556855810-ac404aa91e85?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Vodka", amount: 40.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Cranberry Juice", amount: 40.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Orange Liqueur", amount: 15.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Lemon", amount: 1.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Whiskey Sour",
            description: "balanced sour whiskey cocktail",
            imageUrl: "https://images.unsplash.com/photo-1713720441159-466472b29b54?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Whiskey", amount: 50.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Lemon", amount: 1.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 12.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 4.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Pina Colada",
            description: "tropical rum and coconut cocktail",
            imageUrl: "https://images.unsplash.com/photo-1607644536940-6c300b5784c5?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Rum", amount: 50.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Pineapple Juice", amount: 120.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Coconut Milk", amount: 100.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 6.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Virgin Pina Colada",
            description: "nonalcoholic pineapple coconut drink",
            imageUrl: "https://images.unsplash.com/photo-1607644536940-6c300b5784c5?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Pineapple Juice", amount: 150.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Coconut Milk", amount: 120.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Honey", amount: 10.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 6.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Lemon Iced Tea",
            description: "chilled black tea with lemon",
            imageUrl: "https://images.unsplash.com/photo-1599390719613-912787a6e65a?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Black Tea", amount: 250.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Lemon", amount: 1.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 12.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Ice", amount: 5.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Berry Smoothie",
            description: "mixed berry yogurt smoothie",
            imageUrl: "https://images.unsplash.com/photo-1643470758221-45463a3d210a?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:05:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Strawberry", amount: 4.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Blueberries", amount: 40.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Raspberries", amount: 40.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Yogurt", amount: 180.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Cheesecake Cup",
            description: "no-bake cream cheese dessert cup",
            imageUrl: "https://images.unsplash.com/photo-1561804558-fcfafc5a963e?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:12:00"),
            type: (RecipeType)3,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Cream Cheese", amount: 200.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 80.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 35.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Vanilla Extract", amount: 5.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Whipped Cream", amount: 40.0, unit: (MeasurementUnit)1),
            ]),

        new RecipeSeedItem(
            name: "Tiramisu Cup",
            description: "coffee layered mascarpone dessert",
            imageUrl: "https://images.unsplash.com/photo-1766232333746-b0a2697d6d0d?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:15:00"),
            type: (RecipeType)3,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Mascarpone", amount: 200.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Ladyfingers", amount: 8.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Espresso Shot", amount: 2.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Cocoa Powder", amount: 10.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 30.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Pancake Batter",
            description: "classic sweet pancake base",
            imageUrl: "https://images.unsplash.com/photo-1583611177793-a31c8df1d67f?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:08:00"),
            type: (RecipeType)3,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Flour", amount: 200.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 250.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Egg", amount: 2.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Butter", amount: 40.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Baking Powder", amount: 8.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Banana Oat Shake",
            description: "filling breakfast-style shake",
            imageUrl: "https://images.unsplash.com/photo-1740637372899-e27569049917?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Banana", amount: 1.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Oats", amount: 40.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Milk", amount: 250.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Honey", amount: 8.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Chocolate Cake Batter",
            description: "rich cocoa cake preparation",
            imageUrl: "https://images.unsplash.com/photo-1607662256021-751dc3939f2b?q=80&w=1174&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:12:00"),
            type: (RecipeType)3,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Flour", amount: 220.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Cocoa Powder", amount: 45.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Egg", amount: 3.0, unit: (MeasurementUnit)0),
                new RecipeIngredientSeedItem(ingredientName: "Butter", amount: 90.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Sugar", amount: 160.0, unit: (MeasurementUnit)3),
            ]),

        new RecipeSeedItem(
            name: "Honey Ginger Tea",
            description: "warming tea with ginger and honey",
            imageUrl: "https://images.unsplash.com/photo-1606695889004-f7850b9bfb81?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:04:00"),
            type: (RecipeType)4,
            ingredients:
            [
                new RecipeIngredientSeedItem(ingredientName: "Ginger", amount: 15.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Honey", amount: 12.0, unit: (MeasurementUnit)3),
                new RecipeIngredientSeedItem(ingredientName: "Water", amount: 250.0, unit: (MeasurementUnit)1),
                new RecipeIngredientSeedItem(ingredientName: "Lemon", amount: 1.0, unit: (MeasurementUnit)0),
            ]),

        new RecipeSeedItem(
            name: "Daily Soup",
            description: "soup from daily menu, can be vegetable, meat or fish based",
            imageUrl: null,
            duration: TimeSpan.Parse("00:20:00"),
            type: (RecipeType)2,
            ingredients:
            [
            ]),

        new RecipeSeedItem(
            name: "Fried Cheese",
            description: "Biggest cheese slice in Brno, served with tartar sauce",
            imageUrl: "https://images.unsplash.com/photo-1728468314335-3f973a10e53d?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
            duration: TimeSpan.Parse("00:15:00"),
            type: (RecipeType)1,
            ingredients:
            [
            ]),
    ];
}

internal sealed record RecipeSeedItem(
    string name,
    string? description,
    string? imageUrl,
    TimeSpan duration,
    RecipeType type,
    IReadOnlyList<RecipeIngredientSeedItem> ingredients);

internal sealed record RecipeIngredientSeedItem(string ingredientName, double amount, MeasurementUnit unit);
