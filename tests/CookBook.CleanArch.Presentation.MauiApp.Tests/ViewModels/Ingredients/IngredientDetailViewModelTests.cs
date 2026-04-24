using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Ingredients;

public class IngredientDetailViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task LoadDataAsync_Should_Load_Ingredient_And_Recipes()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var vm = sp.GetRequiredService<IngredientDetailViewModel>();
            vm.Id = IngredientTestSeeds.Lemon.Id;

            // Act
            await vm.OnAppearingAsync();

            // Assert
            Assert.NotNull(vm.Ingredient);
            Assert.Equal(IngredientTestSeeds.Lemon.Name, vm.Ingredient!.Name);
        });
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Ingredient_And_Navigate_Back()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<IngredientDetailViewModel>();

            vm.Id = IngredientTestSeeds.IngredientForTestOfDelete.Id;
            await vm.OnAppearingAsync();

            // Act
            await vm.DeleteCommand.ExecuteAsync(null);

            // Assert DB
            Assert.False(GetDbContext(sp).Ingredients.Any(i => i.Id == vm.Id));

            // Assert navigation
            Assert.True(navigation.BackNavigationCalled);
        });
    }

    [Fact]
    public async Task DeleteAsync_Should_Show_Alert_When_Delete_Fails()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var alert = (TestAlertService)sp.GetRequiredService<IAlertService>();
            var vm = sp.GetRequiredService<IngredientDetailViewModel>();

            vm.Id = IngredientTestSeeds.UsedInSingleRecipe.Id;
            await vm.OnAppearingAsync();

            // Act
            await vm.DeleteCommand.ExecuteAsync(null);

            // Assert
            Assert.True(alert.WasCalled);
        });
    }

    [Fact]
    public async Task GoToEditAsync_Should_Navigate()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<IngredientDetailViewModel>();

            vm.Id = new IngredientId(Guid.NewGuid());

            // Act
            await vm.GoToEditCommand.ExecuteAsync(null);

            // Assert
            Assert.True(navigation.Navigated); // see note below
        });
    }
}
