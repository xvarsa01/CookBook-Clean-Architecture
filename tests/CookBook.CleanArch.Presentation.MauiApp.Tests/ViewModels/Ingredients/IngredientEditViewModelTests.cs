using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Ingredients;

public class IngredientEditViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Ingredient_For_Edit()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientEditViewModel>();
            vm.Id = IngredientTestSeeds.Lemon.Id;

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.NotNull(vm.Ingredient);
            Assert.Equal(IngredientTestSeeds.Lemon.Name, vm.Ingredient!.Name);
        });
    }

    [Fact]
    public async Task LoadDataAsync_Should_Do_Nothing_When_Id_Is_Empty()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientEditViewModel>();
            vm.Id = new IngredientId(Guid.Empty);

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.Equal(IngredientFormModel.Empty.Name, vm.Ingredient.Name);
            Assert.Equal(IngredientFormModel.Empty.Description, vm.Ingredient.Description);
            Assert.Equal(IngredientFormModel.Empty.ImageUrl, vm.Ingredient.ImageUrl);
        });
    }

    [Fact]
    public async Task SaveAsync_Should_Update_Ingredient_And_Navigate_Back()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();

            var vm = sp.GetRequiredService<IngredientEditViewModel>();
            vm.Id = IngredientTestSeeds.IngredientForTestOfUpdate.Id;

            await vm.OnAppearingAsync();

            vm.Ingredient.Name = "updated name";
            vm.Ingredient.Description = "updated description";

            // Act
            await vm.SaveCommand.ExecuteAsync(null);

            // Assert DB
            var db = GetDbContext(sp);
            var updated = db.Ingredients.Single(i => i.Id == vm.Id);

            Assert.Equal("updated name", updated.Name);
            Assert.Equal("updated description", updated.Description);

            // Assert navigation
            Assert.True(navigation.BackNavigationCalled);
        });
    }

    [Fact]
    public async Task SaveAsync_Should_Not_Save_When_Invalid()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientEditViewModel>();
            vm.Id = IngredientTestSeeds.IngredientForTestOfUpdate.Id;

            await vm.OnAppearingAsync();

            vm.Ingredient.Name = string.Empty;

            var db = GetDbContext(sp);
            var before = db.Ingredients.Single(i => i.Id == vm.Id).Name;

            // Act
            await vm.SaveCommand.ExecuteAsync(null);

            var after = db.Ingredients.Single(i => i.Id == vm.Id).Name;

            // Assert unchanged
            Assert.Equal(before, after);
        });
    }
}
