using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Recipes;

public class RecipeListViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Seeded_Recipes()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeListViewModel>();

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.NotNull(vm.Recipes);
            Assert.True(vm.Recipes.Count > 0);
        });
    }

    [Fact]
    public async Task GoToCreateAsync_Should_Navigate_To_Create_Page()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeListViewModel>();

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
            var vm = sp.GetRequiredService<RecipeListViewModel>();

            var recipe = RecipeTestSeeds.RecipeWithSingleIngredient();

            // Act
            await vm.GoToDetailCommand.ExecuteAsync(recipe.Id);

            // Assert
            Assert.True(navigation.Navigated);
        });
    }
    
    [Fact]
    public async Task LoadPage_Should_Set_TotalItemsCount()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeListViewModel>();

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.True(vm.TotalItemsCount > 0);
        });
    }
}
