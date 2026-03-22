using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;
using CookBook.Clean.Core.RecipeRoot.ValueObjects;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests.RecipeRoot;

public class RecipeRootRecipeTests
{
    [Fact]
    public void Creating_Recipe_With_Valid_Initial_State_Should_Create()
    {
        // Act
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);
        
        // Assert
        Assert.NotEqual(Guid.Empty, recipe.Id);
        Assert.Equal("Cappuccino", recipe.Name.Value);
        Assert.Equal("with oak mil", recipe.Description);
        Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg", recipe.ImageUrl?.Value);
        Assert.Empty(recipe.Ingredients);
    }
    
    [Fact]
    public void Creating_Recipe_WithOut_DescriptionAndImage_Should_Create()
    {
        // Act
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), null, null, new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);
        
        // Assert
        Assert.NotEqual(Guid.Empty, recipe.Id);
        Assert.Equal("Cappuccino", recipe.Name.Value);
        Assert.Null(recipe.Description);
        Assert.Null(recipe.ImageUrl);
        Assert.Empty(recipe.Ingredients);
    }
    
    [Fact]
    public void Creating_Recipe_With_Empty_Name_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
                new RecipeEntity(new RecipeName(""), null, null, new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink)
        );
    }
    
    [Fact]
    public void Creating_Recipe_With_Short_Name_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new RecipeEntity(new RecipeName("AB"), null, null, new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink)
        );
    }
    
    [Fact]
    public void Creating_Recipe_With_Negative_Duration_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new RecipeEntity(new RecipeName("Cappuccino"), null, null, new RecipeDuration(TimeSpan.FromMinutes(-1)), RecipeType.Drink)
        );
    }
    
    [Fact]
    public void Creating_Recipe_With_Zero_Duration_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new RecipeEntity(new RecipeName("Cappuccino"), null, null, new RecipeDuration(TimeSpan.Zero), RecipeType.Drink)
        );
    }
    
    [Fact]
    public void Updating_RecipeName_Should_Update_Name()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateName(new RecipeName("New"));

        Assert.Equal("New", recipe.Name.Value);
    }
    
    [Fact]
    public void Updating_RecipeName_ToEmpty_Should_Throw()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        Assert.Throws<ArgumentException>(() =>
                recipe.UpdateName(new RecipeName(""))
        );
    }
    
    [Fact]
    public void Updating_RecipeName_ToShort_Should_Throw()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        Assert.Throws<ArgumentException>(() =>
            recipe.UpdateName(new RecipeName("AB"))
        );
    }
    [Fact]
    public void Updating_RecipeName_To_Same_Value_Should_Not_Change()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateName(new RecipeName("Cappuccino"));

        Assert.Equal("Cappuccino", recipe.Name.Value);
    }
    
    
    [Fact]
    public void Updating_RecipeDescription_Should_Update_Description()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), null, null, new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.None);

        recipe.UpdateDescription("New");

        Assert.Equal("New", recipe.Description);
    }
    
    [Fact]
    public void Updating_RecipeDescription_To_Same_Value_Should_Not_Change()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "Same", null, new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.None);

        recipe.UpdateDescription("Same");

        Assert.Equal("Same", recipe.Description);
    }
    
    
    [Fact]
    public void Updating_RecipeImageUrl_Should_Update_ImageUrl()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateRest(new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/Cappuccino_in_original.jpg/1920px-Cappuccino_in_original.jpg"), null, null);

        Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/Cappuccino_in_original.jpg/1920px-Cappuccino_in_original.jpg", recipe.ImageUrl?.Value);
    }
    
    [Fact]
    public void Updating_RecipeImageUrl_To_Same_Value_Should_Not_Change()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateRest(new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), null, null);

        Assert.Equal("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg", recipe.ImageUrl?.Value);
    }
    
    
    [Fact]
    public void Updating_RecipeDuration_Should_Update_Duration()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateRest(null, new RecipeDuration(TimeSpan.FromMinutes(10)), null);

        Assert.Equal(TimeSpan.FromMinutes(10), recipe.Duration.Value);
    }
    
    [Fact]
    public void Updating_RecipeDuration_To_Same_Value_Should_Not_Change()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateRest(null, new RecipeDuration(TimeSpan.FromMinutes(5)), null);

        Assert.Equal(TimeSpan.FromMinutes(5), recipe.Duration.Value);
    }
    
    [Fact]
    public void Updating_RecipeDuration_To_Negative_Value_Should_Throw()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        Assert.Throws<ArgumentException>(() =>
            recipe.UpdateRest(null, new RecipeDuration(TimeSpan.FromMinutes(-1)), null)
        );
    }
    
    
    [Fact]
    public void Updating_RecipeType_Should_Update_Type()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateRest(null, null, RecipeType.Dessert);

        Assert.Equal(RecipeType.Dessert, recipe.Type);
    }
    
    [Fact]
    public void Updating_RecipeType_To_Same_Value_Should_Not_Change()
    {
        var recipe = new RecipeEntity(new RecipeName("Cappuccino"), "with oak mil", new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/b/b8/Cappuccino_milk_froth.jpg"), new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        recipe.UpdateRest(null, new RecipeDuration(TimeSpan.FromMinutes(5)), RecipeType.Drink);

        Assert.Equal(RecipeType.Drink, recipe.Type);
    }

}
