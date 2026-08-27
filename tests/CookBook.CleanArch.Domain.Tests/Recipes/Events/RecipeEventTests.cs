using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Recipes.Events;

public class RecipeEventTests
{
    [Fact]
    public void Updating_RecipeName_Should_Trigger_RecipeNameUpdatedEvent()
    {
        var recipe = RecipeTestData.CreateRecipe();
        var originalName = recipe.Name;

        recipe.UpdateName(RecipeName.CreateObject("New name").Value);
        
        var evt = Assert.Single(recipe.GetDomainEvents().OfType<RecipeNameUpdatedEvent>());
        Assert.Equal(recipe.Id, evt.RecipeId);
        Assert.Equal(originalName, evt.OldName);
        Assert.Equal("New name", evt.NewName);
    }
    
    [Fact]
    public void Updating_RecipeDescription_Should_Trigger_RecipeDescriptionUpdatedEvent()
    {
        var recipe = RecipeTestData.CreateRecipe();
        var originalDescription = recipe.Description;
        
        recipe.UpdateDescription("New description");

        var evt = Assert.Single(recipe.GetDomainEvents().OfType<RecipeDescriptionUpdatedEvent>());
        Assert.Equal(recipe.Id, evt.RecipeId);
        Assert.Equal(originalDescription, evt.OldDescription);
        Assert.Equal("New description", evt.NewDescription);
    }
    
    [Fact]
    public void Deleting_Recipe_Should_Trigger_RecipeDeletedEvent()
    {
        var recipe = RecipeTestData.CreateRecipe();
        
        recipe.Delete();

        Assert.Single(recipe.GetDomainEvents().OfType<RecipeDeletedEvent>());
    }
}
