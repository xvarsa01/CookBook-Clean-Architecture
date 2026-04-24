using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.Domain.Tests.Shared;

public class AggregateRootBaseTests
{
    [Fact]
    public void Clearing_DomainEvents_Should_Remove_All_Events()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        recipe.UpdateName(RecipeName.CreateObject("New").Value);
        recipe.UpdateDescription("New description");
        recipe.Delete();

        Assert.NotEmpty(recipe.GetDomainEvents());

        // Act
        recipe.ClearDomainEvents();

        // Assert
        Assert.Empty(recipe.GetDomainEvents());
    }
    
    [Fact]
    public void Clearing_DomainEvents_On_Empty_Collection_Should_Not_Throw()
    {
        var recipe = RecipeTestSeeds.MinimalisticRecipe();

        recipe.ClearDomainEvents();

        Assert.Empty(recipe.GetDomainEvents());
    }
}
