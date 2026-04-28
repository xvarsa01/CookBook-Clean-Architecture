using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Ingredients;

public class IngredientListViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Seeded_Ingredients()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientListViewModel>();

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.NotNull(vm.Ingredients);
            Assert.True(vm.Ingredients.Count > 0);
        });
    }

    [Fact]
    public async Task GoToCreateAsync_Should_Navigate_To_Create_Page()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<IngredientListViewModel>();

            // Act
            await vm.GoToCreateCommand.ExecuteAsync(null);

            // Assert
            Assert.True(navigation.Navigated);
        });
    }

    [Fact]
    public async Task GoToDetailAsync_Should_Navigate_To_Detail_Page()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<IngredientListViewModel>();

            var ingredient = IngredientTestSeeds.Lemon;

            // Act
            await vm.GoToDetailCommand.ExecuteAsync(ingredient.Id);

            // Assert
            Assert.True(navigation.Navigated);
        });
    }
    
    [Fact]
    public async Task LoadPage_Should_Set_TotalItemsCount()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientListViewModel>();

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.True(vm.TotalItemsCount > 0);
        });
    }
}
