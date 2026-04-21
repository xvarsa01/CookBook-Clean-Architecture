using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Events;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.DomainTests.Recipes.Events;

public class RecipeEventTests
{
    [Fact]
    public void Updating_RecipeName_Should_Trigger_RecipeNameUpdatedEvent()
    {
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        recipe.UpdateName(RecipeName.CreateObject("New").Value);
        
        var evt = Assert.Single(recipe.GetDomainEvents().OfType<RecipeNameUpdatedEvent>());
        Assert.Equal(recipe.Id, evt.RecipeId);
        Assert.Equal(RecipeTestSeeds.MinimalisticRecipe().Name, evt.OldName);
        Assert.Equal("New", evt.NewName);
    }
    
    [Fact]
    public void Updating_RecipeDescription_Should_Trigger_RecipeDescriptionUpdatedEvent()
    {
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        
        recipe.UpdateDescription("New description");

        var evt = Assert.Single(recipe.GetDomainEvents().OfType<RecipeDescriptionUpdatedEvent>());
        Assert.Equal(recipe.Id, evt.RecipeId);
        Assert.Equal(RecipeTestSeeds.MinimalisticRecipe().Description, evt.OldDescription);
        Assert.Equal("New description", evt.NewDescription);
    }
    
    [Fact]
    public void Deleting_Recipe_Should_Trigger_RecipeDeletedEvent()
    {
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        
        recipe.Delete();

        Assert.Single(recipe.GetDomainEvents().OfType<RecipeDeletedEvent>());
    }
}
