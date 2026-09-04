using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Presentation.MauiApp.IntegrationTests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApp.IntegrationTests.ViewModels.Recipes;

public class RecipeDetailViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Recipe_Detail()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<RecipeDetailViewModel>();
            var seeded = Recipes.WithSingleIngredient;
            vm.Id = seeded.Id;

            await vm.OnAppearingAsync();

            Assert.NotNull(vm.Recipe);
            Assert.Equal(seeded.Name, vm.Recipe!.Name);
            Assert.Single(vm.Recipe.Ingredients);
        });
    }

    [Fact]
    public async Task DeleteCommand_Should_Delete_Recipe_And_Navigate_Back()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeDetailViewModel>();

            var seeded = Recipes.WithTwoIngredients;
            vm.Id = seeded.Id;
            await vm.OnAppearingAsync();

            await vm.DeleteCommand.ExecuteAsync(null);

            Assert.False(GetDbContext(sp).Recipes.Any(r => r.Id == vm.Id));
            Assert.True(navigation.BackNavigationCalled);
        });
    }

    [Fact]
    public async Task GoToEditCommand_Should_Navigate_When_Recipe_Is_Loaded()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<RecipeDetailViewModel>();

            var seeded = Recipes.WithSingleIngredient;
            vm.Id = seeded.Id;
            await vm.OnAppearingAsync();

            await vm.GoToEditCommand.ExecuteAsync(null);

            Assert.True(navigation.Navigated);
        });
    }
}
