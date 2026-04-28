using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Recipes;

public class RecipeEditViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Recipe_For_Edit()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);
            vm.Id = actual.Id;

            await vm.OnAppearingAsync();

            Assert.NotNull(vm.Recipe);
            Assert.Equal(seeded.Name, vm.Recipe!.Name);
            Assert.NotEmpty(vm.Recipe.Ingredients);
        });
    }

    [Fact]
    public async Task AddNewIngredientCommand_Should_Add_Pending_Ingredient_And_Send_Add_Message()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var messenger = (TestMessengerService)sp.GetRequiredService<IMessengerService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            vm.SelectedNewIngredient = vm.Ingredients.Single(x => x.Id.Value == IngredientTestSeeds.Lemon.Id.Value);
            vm.IngredientAmountNew.Amount = 12;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.Recipe.Ingredients.Count);
            var added = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Lemon.Id.Value);
            Assert.Equal(Guid.Empty, added.RecipeIngredientId);
            Assert.Equal(IngredientTestSeeds.Lemon.Name, added.IngredientName);
            Assert.Equal(12, added.Amount);
            Assert.Equal(MeasurementUnit.Pieces, added.Unit);
            Assert.Equal(Guid.Empty, vm.IngredientAmountNew.IngredientId);
            Assert.Equal(0, vm.IngredientAmountNew.Amount);
            Assert.Equal(MeasurementUnit.None, vm.IngredientAmountNew.Unit);
            Assert.Null(vm.SelectedNewIngredient);
            Assert.Contains(messenger.SentMessages, message => message is RecipeIngredientAddMessage);
        });
    }

    [Fact]
    public async Task AddNewIngredientCommand_Should_Not_Add_When_Ingredient_Is_Not_Selected()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var messenger = (TestMessengerService)sp.GetRequiredService<IMessengerService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            vm.IngredientAmountNew.Amount = 12;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            Assert.Single(vm.Recipe.Ingredients);
            Assert.Empty(messenger.SentMessages);
        });
    }

    [Fact]
    public async Task UpdateIngredientCommand_Should_Update_Pending_Add_Request_For_Tracked_Ingredient()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var messenger = (TestMessengerService)sp.GetRequiredService<IMessengerService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            vm.SelectedNewIngredient = vm.Ingredients.Single(x => x.Id.Value == IngredientTestSeeds.Lemon.Id.Value);
            vm.IngredientAmountNew.Amount = 12;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            var added = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Lemon.Id.Value);
            added.Amount = 25;
            added.Unit = MeasurementUnit.Ml;

            vm.UpdateIngredientAsyncCommand.Execute(added);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var saved = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            var persistedAdded = saved.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Lemon.Id);

            Assert.Equal(25, persistedAdded.Amount.Value);
            Assert.Equal(MeasurementUnit.Ml, persistedAdded.Unit);
            Assert.Contains(messenger.SentMessages, message => message is RecipeIngredientEditMessage);
        });
    }

    [Fact]
    public async Task UpdateIngredientCommand_Should_Queue_Update_For_Persisted_Ingredient()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            var model = vm.Recipe.Ingredients.Single();
            model.Amount = 250;
            model.Unit = MeasurementUnit.Pieces;

            vm.UpdateIngredientAsyncCommand.Execute(model);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var saved = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            var persisted = saved.Ingredients.Single();

            Assert.Equal(250, persisted.Amount.Value);
            Assert.Equal(MeasurementUnit.Pieces, persisted.Unit);
        });
    }

    [Fact]
    public async Task UpdateIngredientCommand_Should_Ignore_Removed_Ingredient()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeWithTwoIngredients();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            var water = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Water.Id.Value);
            await vm.RemoveIngredientCommand.ExecuteAsync(water);

            water.Amount = 999;
            water.Unit = MeasurementUnit.Pieces;
            vm.UpdateIngredientAsyncCommand.Execute(water);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var saved = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            Assert.Single(saved.Ingredients);
            var remaining = saved.Ingredients.Single();
            Assert.Equal(IngredientTestSeeds.Lemon.Id.Value, remaining.IngredientId.Value);
            Assert.Equal(1, remaining.Amount.Value);
            Assert.Equal(MeasurementUnit.None, remaining.Unit);
        });
    }

    [Fact]
    public async Task RemoveIngredientCommand_Should_Remove_Pending_Add_And_Send_Delete_Message()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var messenger = (TestMessengerService)sp.GetRequiredService<IMessengerService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            vm.SelectedNewIngredient = vm.Ingredients.Single(x => x.Id.Value == IngredientTestSeeds.Lemon.Id.Value);
            vm.IngredientAmountNew.Amount = 12;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            var added = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Lemon.Id.Value);

            await vm.RemoveIngredientCommand.ExecuteAsync(added);

            Assert.Single(vm.Recipe.Ingredients);
            Assert.Contains(messenger.SentMessages, message => message is RecipeIngredientDeleteMessage);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var saved = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            Assert.Single(saved.Ingredients);
            Assert.Equal(IngredientTestSeeds.Water.Id.Value, saved.Ingredients.Single().IngredientId.Value);
        });
    }

    [Fact]
    public async Task RemoveIngredientCommand_Should_Track_Persisted_Ingredient_Removal_And_Cancel_Pending_Update()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeWithTwoIngredients();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            var water = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Water.Id.Value);
            water.Amount = 777;
            water.Unit = MeasurementUnit.Pieces;
            vm.UpdateIngredientAsyncCommand.Execute(water);

            await vm.RemoveIngredientCommand.ExecuteAsync(water);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var saved = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            Assert.Single(saved.Ingredients);
            var remaining = saved.Ingredients.Single();
            Assert.Equal(IngredientTestSeeds.Lemon.Id.Value, remaining.IngredientId.Value);
            Assert.Equal(1, remaining.Amount.Value);
            Assert.Equal(MeasurementUnit.None, remaining.Unit);
        });
    }

    [Fact]
    public async Task SaveRecipeCommand_Should_Apply_Pending_Ingredient_Changes()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeWithTwoIngredients();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            var water = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Water.Id.Value);
            var lemon = vm.Recipe.Ingredients.Single(x => x.IngredientId == IngredientTestSeeds.Lemon.Id.Value);

            await vm.RemoveIngredientCommand.ExecuteAsync(water);

            vm.SelectedNewIngredient = vm.Ingredients.Single(x => x.Id.Value == IngredientTestSeeds.IngredientNotUsedInAnyRecipe.Id.Value);
            vm.IngredientAmountNew.Amount = 5;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;
            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            lemon.Amount = 333;
            lemon.Unit = MeasurementUnit.Ml;
            vm.UpdateIngredientAsyncCommand.Execute(lemon);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var saved = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            Assert.True(navigation.BackNavigationCalled);
            Assert.Equal(2, saved.Ingredients.Count);
            Assert.DoesNotContain(saved.Ingredients, x => x.IngredientId.Value == IngredientTestSeeds.Water.Id.Value);

            var persistedLemon = saved.Ingredients.Single(x => x.IngredientId.Value == IngredientTestSeeds.Lemon.Id.Value);
            Assert.Equal(333, persistedLemon.Amount.Value);
            Assert.Equal(MeasurementUnit.Ml, persistedLemon.Unit);

            var persistedNewIngredient = saved.Ingredients.Single(x => x.IngredientId.Value == IngredientTestSeeds.IngredientNotUsedInAnyRecipe.Id.Value);
            Assert.Equal(5, persistedNewIngredient.Amount.Value);
            Assert.Equal(MeasurementUnit.Pieces, persistedNewIngredient.Unit);
        });
    }

    [Fact]
    public async Task SaveRecipeCommand_Should_Not_Save_When_Pending_Addition_Fails()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeFullWithMaximumIngredients();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            vm.SelectedNewIngredient = vm.Ingredients.Single(x => x.Id.Value == IngredientTestSeeds.Lemon.Id.Value);
            vm.IngredientAmountNew.Amount = 1;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            var before = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var after = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Id == vm.Id);

            Assert.False(navigation.BackNavigationCalled);
            Assert.Equal(before.Ingredients.Count, after.Ingredients.Count);
            Assert.DoesNotContain(after.Ingredients, x => x.IngredientId.Value == IngredientTestSeeds.Lemon.Id.Value && x.Amount.Value == 1);
        });
    }

    [Fact]
    public async Task SaveRecipeCommand_Should_Update_Recipe_And_Navigate_Back()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            var seeded = RecipeTestSeeds.RecipeForTestOfUpdate();
            var actual = SeededRecipes.Single(r => r.Name == seeded.Name);

            vm.Id = actual.Id;
            await vm.OnAppearingAsync();

            vm.Recipe.Name = "updated recipe name";
            vm.Recipe.Description = "updated recipe description";
            vm.Recipe.Duration = TimeSpan.FromMinutes(45);
            vm.Recipe.RecipeType = RecipeType.Dessert;

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var updated = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .SingleAsync(r => r.Id == vm.Id);

            Assert.Equal("updated recipe name", updated.Name);
            Assert.Equal("updated recipe description", updated.Description);
            Assert.Equal(TimeSpan.FromMinutes(45), updated.Duration);
            Assert.Equal(RecipeType.Dessert, updated.Type);
            Assert.True(navigation.BackNavigationCalled);
        });
    }
    
    [Fact]
    public async Task SaveRecipeCommand_Should_Not_Save_When_Invalid()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeEditViewModel>();
            vm.Id = GetSeededRecipeByName(RecipeTestSeeds.RecipeForTestOfUpdate().Name).Id;

            await vm.OnAppearingAsync();

            vm.Recipe.Name = string.Empty;

            var db = GetDbContext(sp);
            var before = db.Recipes.Single(i => i.Id == vm.Id).Name;

            // Act
            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var after = db.Recipes.Single(i => i.Id == vm.Id).Name;

            // Assert unchanged
            Assert.Equal(before, after);
        });
    }
}
