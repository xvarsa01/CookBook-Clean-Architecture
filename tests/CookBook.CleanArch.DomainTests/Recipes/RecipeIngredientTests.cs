using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;

namespace CookBook.CleanArch.DomainTests.Recipes;

public class RecipeIngredientTests
{
    [Fact]
    public void AddIngredient_WhenRecipeHasCapacity_ShouldReturnSuccessAndAppendEntry()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var ingredientId = new IngredientId(Guid.NewGuid());
        var countBefore = recipe.Ingredients.Count;

        // Act
        var result = recipe.AddIngredient(
            ingredientId,
            IngredientAmount.CreateObject(100).Value,
            MeasurementUnit.Ml);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(countBefore + 1, recipe.Ingredients.Count);

        var addedEntry = recipe.Ingredients.Single(i => i.Id == result.Value);
        Assert.Equal(ingredientId, addedEntry.IngredientId);
        Assert.Equal(100, addedEntry.Amount.Value);
        Assert.Equal(MeasurementUnit.Ml, addedEntry.Unit);
    }

    [Fact]
    public void AddIngredient_WhenRecipeIsFull_ShouldReturnFailureWithMaximumIngredientsError()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeFullWithMaximumIngredients();
        var ingredientId = new IngredientId(Guid.NewGuid());
        var before = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();

        // Act
        var result = recipe.AddIngredient(
            ingredientId,
            IngredientAmount.CreateObject(100).Value,
            MeasurementUnit.Ml);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMaximumNumberOfIngredientsError(recipe.Id), result.Error);

        var after = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();
        Assert.Equal(before, after);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void AddIngredient_WhenAmountIsNotPositive_ShouldFailIngredientAmountCreation(decimal invalidAmount)
    {
        // Arrange/Act
        var amountResult = IngredientAmount.CreateObject(invalidAmount);

        // Assert
        Assert.True(amountResult.IsFailure);
    }

    
    // UPDATES:
    
    [Fact]
    public void UpdateIngredientEntry_WhenEntryExists_ShouldReturnSuccessAndUpdateOnlyTargetEntry()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var targetEntry = recipe.Ingredients.First();
        var untouchedEntry = recipe.Ingredients.Last();
        var untouchedAmountBefore = untouchedEntry.Amount;
        var untouchedUnitBefore = untouchedEntry.Unit;

        // Act
        var result = recipe.UpdateIngredientEntry(
            targetEntry.Id,
            IngredientAmount.CreateObject(123_456).Value,
            MeasurementUnit.Kg);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, recipe.Ingredients.Count);

        var updatedTarget = recipe.Ingredients.Single(i => i.Id == targetEntry.Id);
        Assert.Equal(123_456, updatedTarget.Amount.Value);
        Assert.Equal(MeasurementUnit.Kg, updatedTarget.Unit);

        var untouchedAfter = recipe.Ingredients.Single(i => i.Id == untouchedEntry.Id);
        Assert.Equal(untouchedAmountBefore, untouchedAfter.Amount);
        Assert.Equal(untouchedUnitBefore, untouchedAfter.Unit);
    }

    [Fact]
    public void UpdateIngredientEntry_WhenEntryDoesNotExist_ShouldReturnFailureWithEntryNotFoundError()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var missingEntryId = new RecipeIngredientId(Guid.NewGuid());

        // Act
        var result = recipe.UpdateIngredientEntry(
            missingEntryId,
            IngredientAmount.CreateObject(250).Value,
            MeasurementUnit.Ml);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(missingEntryId, recipe.Id), result.Error);
    }

    [Fact]
    public void UpdateIngredientEntry_WhenEntryDoesNotExist_ShouldNotChangeIngredientState()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var before = recipe.Ingredients.Select(i => (i.Id, i.Amount, i.Unit)).ToList();

        // Act
        var result = recipe.UpdateIngredientEntry(
            new RecipeIngredientId(Guid.NewGuid()),
            IngredientAmount.CreateObject(777).Value,
            MeasurementUnit.G);

        // Assert
        Assert.True(result.IsFailure);
        var after = recipe.Ingredients.Select(i => (i.Id, i.Amount, i.Unit)).ToList();
        Assert.Equal(before, after);
    }

    [Fact]
    public void UpdateIngredientEntry_WhenRecipeHasDuplicateIngredientIds_ShouldUpdateOnlyMatchingEntryId()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithDuplicateIngredientEntries();
        var duplicatedIngredientId = recipe.Ingredients
            .GroupBy(i => i.IngredientId)
            .Single(g => g.Count() > 1)
            .Key;

        var entriesWithSameIngredientId = recipe.Ingredients
            .Where(i => i.IngredientId == duplicatedIngredientId)
            .ToList();

        var targetEntry = entriesWithSameIngredientId[0];
        var otherEntry = entriesWithSameIngredientId[1];
        var otherAmountBefore = otherEntry.Amount;
        var otherUnitBefore = otherEntry.Unit;

        // Act
        var result = recipe.UpdateIngredientEntry(
            targetEntry.Id,
            IngredientAmount.CreateObject(321).Value,
            MeasurementUnit.G);

        // Assert
        Assert.True(result.IsSuccess);

        var updatedTarget = recipe.Ingredients.Single(i => i.Id == targetEntry.Id);
        Assert.Equal(321, updatedTarget.Amount.Value);
        Assert.Equal(MeasurementUnit.G, updatedTarget.Unit);

        var untouchedOther = recipe.Ingredients.Single(i => i.Id == otherEntry.Id);
        Assert.Equal(otherAmountBefore, untouchedOther.Amount);
        Assert.Equal(otherUnitBefore, untouchedOther.Unit);
    }

    [Fact]
    public void UpdateIngredientEntry_WhenRecipeHasSingleIngredient_ShouldReturnSuccessAndKeepCount()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        var entry = recipe.Ingredients.Single();

        // Act
        var result = recipe.UpdateIngredientEntry(
            entry.Id,
            IngredientAmount.CreateObject(42).Value,
            MeasurementUnit.Pieces);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(recipe.Ingredients);
        Assert.Equal(42, recipe.Ingredients.Single().Amount.Value);
        Assert.Equal(MeasurementUnit.Pieces, recipe.Ingredients.Single().Unit);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void UpdateIngredientEntry_WhenAmountIsNotPositive_ShouldFailIngredientAmountCreation(decimal invalidAmount)
    {
        // Arrange/Act
        var amountResult = IngredientAmount.CreateObject(invalidAmount);

        // Assert
        Assert.True(amountResult.IsFailure);
    }

    
    // REMOVALS:
    
    [Fact]
    public void RemoveIngredientsByIngredientId_WhenIngredientExistsInMultipleEntries_ShouldRemoveAllMatchingEntries()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithDuplicateIngredientEntries();
        var duplicatedIngredientId = recipe.Ingredients
            .GroupBy(i => i.IngredientId)
            .Single(g => g.Count() > 1)
            .Key;

        var nonTargetIds = recipe.Ingredients
            .Where(i => i.IngredientId != duplicatedIngredientId)
            .Select(i => i.Id)
            .ToList();

        // Act
        var result = recipe.RemoveIngredientsByIngredientId(duplicatedIngredientId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(recipe.Ingredients, i => i.IngredientId == duplicatedIngredientId);
        Assert.All(nonTargetIds, id => Assert.Contains(recipe.Ingredients, i => i.Id == id));
    }

    [Fact]
    public void RemoveIngredientsByIngredientId_WhenIngredientDoesNotExist_ShouldReturnFailureWithNotFoundError()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var missingIngredientId = new IngredientId(Guid.NewGuid());
        var before = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();

        // Act
        var result = recipe.RemoveIngredientsByIngredientId(missingIngredientId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeIngredientByIdNotFoundError(missingIngredientId, recipe.Id), result.Error);

        var after = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();
        Assert.Equal(before, after);
    }

    [Fact]
    public void RemoveIngredientsByIngredientId_WhenRemovalWouldLeaveNoIngredients_ShouldReturnFailureWithMinimumIngredientsError()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        var ingredientId = recipe.Ingredients.Single().IngredientId;
        var before = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();

        // Act
        var result = recipe.RemoveIngredientsByIngredientId(ingredientId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);

        var after = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();
        Assert.Equal(before, after);
    }

    [Fact]
    public void RemoveIngredientByEntryId_WhenEntryExistsInMultiIngredientRecipe_ShouldReturnSuccessAndRemoveOnlyTargetEntry()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var targetEntry = recipe.Ingredients.First();
        var otherEntry = recipe.Ingredients.Last();

        // Act
        var result = recipe.RemoveIngredientByEntryId(targetEntry.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(recipe.Ingredients);
        Assert.DoesNotContain(recipe.Ingredients, i => i.Id == targetEntry.Id);
        Assert.Contains(recipe.Ingredients, i => i.Id == otherEntry.Id);
    }

    [Fact]
    public void RemoveIngredientByEntryId_WhenEntryDoesNotExist_ShouldReturnFailureWithNotFoundError()
    {
        // Arrange
        var recipe = RecipeTestSeeds.RecipeWithTwoIngredients();
        var missingEntryId = new RecipeIngredientId(Guid.NewGuid());
        var before = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();

        // Act
        var result = recipe.RemoveIngredientByEntryId(missingEntryId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeIngredientByEntryIdNotFoundError(missingEntryId, recipe.Id), result.Error);

        var after = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();
        Assert.Equal(before, after);
    }

    [Fact]
    public void RemoveIngredientByEntryId_WhenRecipeHasSingleIngredient_ShouldReturnFailureWithMinimumIngredientsError()
    {
        // Arrange
        var recipe = RecipeTestSeeds.MinimalisticRecipe();
        var onlyEntryId = recipe.Ingredients.Single().Id;
        var before = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();

        // Act
        var result = recipe.RemoveIngredientByEntryId(onlyEntryId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(RecipeErrors.RecipeMinimumNumberOfIngredientsError(recipe.Id), result.Error);

        var after = recipe.Ingredients.Select(i => (i.Id, i.IngredientId, i.Amount, i.Unit)).ToList();
        Assert.Equal(before, after);
    }
}
