using CookBook.CleanArch.Domain.Ingredients;
using CookBook.CleanArch.Presentation.MauiApp.Tests.MockedServices;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using CookBook.CleanArch.Presentation.MauiApplication.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Presentation.MauiApp.Tests.ViewModels.Ingredients;

public class IngredientCreateViewModelTests : MauiTestsBase
{
    [Fact]
    public async Task SaveAsync_Should_Create_Ingredient_And_Navigate_Back()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<IngredientCreateViewModel>();

            vm.Ingredient.Name = "Salt";
            vm.Ingredient.Description = "Basic seasoning";

            await vm.SaveCommand.ExecuteAsync(null);

            var ingredient = GetDbContext(sp).Set<Ingredient>().SingleOrDefault(i => i.Name == "Salt");

            Assert.NotNull(ingredient);
            Assert.Equal("Basic seasoning", ingredient.Description);
            Assert.True(navigation.BackNavigationCalled);
        });
    }

    [Fact]
    public async Task SaveAsync_Should_Not_Create_When_Invalid()
    {
        await ExecuteScopeAsync(async sp =>
        {
            var startCount = GetDbContext(sp).Ingredients.Count();
            
            var navigation = (TestNavigationService)sp.GetRequiredService<INavigationService>();
            var vm = sp.GetRequiredService<IngredientCreateViewModel>();

            vm.Ingredient.Name = string.Empty;

            await vm.SaveCommand.ExecuteAsync(null);

            var finalCount = GetDbContext(sp).Ingredients.Count();
            
            Assert.Equal(startCount, finalCount);
            Assert.False(navigation.BackNavigationCalled);
        });
    }
}
