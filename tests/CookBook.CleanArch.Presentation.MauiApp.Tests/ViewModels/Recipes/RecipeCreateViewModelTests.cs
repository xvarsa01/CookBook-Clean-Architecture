using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes.Enums;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Recipes;

public class RecipeCreateViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Ingredients()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeCreateViewModel>();

            await vm.OnAppearingAsync();

            Assert.NotEmpty(vm.Ingredients);
        });
    }

    [Fact]
    public async Task AddNewIngredientCommand_Should_Add_Selected_Ingredient_And_Reset_Selection()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeCreateViewModel>();

            await vm.OnAppearingAsync();

            vm.SelectedNewIngredient = vm.Ingredients.Single(x => x.Id.Value == IngredientTestSeeds.Lemon.Id.Value);
            vm.IngredientAmountNew.Amount = 120;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            Assert.Single(vm.Recipe.Ingredients);
            var added = vm.Recipe.Ingredients.Single();
            Assert.Equal(IngredientTestSeeds.Lemon.Id.Value, added.IngredientId);
            Assert.Equal(IngredientTestSeeds.Lemon.Name, added.IngredientName);
            Assert.Equal(120, added.Amount);
            Assert.Equal(MeasurementUnit.Pieces, added.Unit);
            Assert.Equal(Guid.Empty, vm.IngredientAmountNew.IngredientId);
            Assert.Equal(0, vm.IngredientAmountNew.Amount);
            Assert.Equal(MeasurementUnit.None, vm.IngredientAmountNew.Unit);
            Assert.Null(vm.SelectedNewIngredient);
        });
    }

    [Fact]
    public async Task AddNewIngredientCommand_Should_Not_Add_When_Ingredient_Is_Not_Selected()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var messenger = (TestMessengerService)sp.GetRequiredService<IMessengerService>();
            var vm = sp.GetRequiredService<RecipeCreateViewModel>();

            await vm.OnAppearingAsync();

            vm.IngredientAmountNew.Amount = 120;
            vm.IngredientAmountNew.Unit = MeasurementUnit.Pieces;

            await vm.AddNewIngredientToRecipeCommand.ExecuteAsync(null);

            Assert.Empty(vm.Recipe.Ingredients);
            Assert.Empty(messenger.SentMessages);
        });
    }

    [Fact]
    public async Task RemoveIngredientCommand_Should_Remove_Ingredient_And_Send_Delete_Message()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var messenger = (TestMessengerService)sp.GetRequiredService<IMessengerService>();
            var vm = sp.GetRequiredService<RecipeCreateViewModel>();

            await vm.OnAppearingAsync();

            var model = new RecipeIngredientListModel
            {
                RecipeIngredientId = Guid.Empty,
                IngredientId = IngredientTestSeeds.Water.Id.Value,
                IngredientName = IngredientTestSeeds.Water.Name,
                IngredientImageUrl = IngredientTestSeeds.Water.ImageUrl?.Value,
                Amount = 50,
                Unit = MeasurementUnit.Ml
            };

            vm.Recipe.Ingredients.Add(model);

            vm.RemoveIngredientCommand.Execute(model);

            Assert.Empty(vm.Recipe.Ingredients);
            Assert.Contains(messenger.SentMessages, message => message is RecipeIngredientDeleteMessage);
        });
    }

    [Fact]
    public async Task SaveRecipeCommand_Should_Create_Recipe_And_Navigate_Back()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeCreateViewModel>();

            await vm.OnAppearingAsync();

            vm.Recipe.Name = "maui created recipe";
            vm.Recipe.Description = "created from tests";
            vm.Recipe.Duration = TimeSpan.FromMinutes(25);
            vm.Recipe.RecipeType = RecipeType.MainDish;
            vm.Recipe.Ingredients.Add(new RecipeIngredientListModel
            {
                RecipeIngredientId = Guid.Empty,
                IngredientId = IngredientTestSeeds.Lemon.Id.Value,
                IngredientName = IngredientTestSeeds.Lemon.Name,
                IngredientImageUrl = IngredientTestSeeds.Lemon.ImageUrl?.Value,
                Amount = 120,
                Unit = MeasurementUnit.Pieces
            });

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            var created = await GetDbContext(sp).Recipes
                .AsNoTracking()
                .Include(r => r.Ingredients)
                .SingleAsync(r => r.Name == vm.Recipe.Name);

            Assert.Equal(vm.Recipe.Description, created.Description);
            Assert.Single(created.Ingredients);
            Assert.True(navigation.BackNavigationCalled);
        });
    }

    [Fact]
    public async Task SaveRecipeCommand_Should_Not_Create_Recipe_When_Invalid()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeCreateViewModel>();

            await vm.OnAppearingAsync();

            vm.Recipe.Name = "invalid recipe";
            vm.Recipe.RecipeType = RecipeType.MainDish;

            var startCount = GetDbContext(sp).Recipes.Count();

            await vm.SaveRecipeCommand.ExecuteAsync(null);

            Assert.Equal(startCount, GetDbContext(sp).Recipes.Count());
            Assert.False(navigation.BackNavigationCalled);
        });
    }
}
