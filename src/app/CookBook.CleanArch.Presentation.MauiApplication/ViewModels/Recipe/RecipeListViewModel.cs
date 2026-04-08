using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Filters;
using CookBook.CleanArch.Application.Models.Recipe;
using CookBook.CleanArch.Application.Queries.Recipes;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Models;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class RecipeListViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelWithPager<RecipeFilter, RecipeSortParameter>(messengerService), IRecipient<RecipeEditMessage>, IRecipient<RecipeDeleteMessage>
{
    [ObservableProperty]
    public partial ObservableCollection<RecipeListResponse> Recipes { get; set; } = [];

    public override RecipeFilter Filter { get; set; } = new();

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await LoadPageAsync();
    }

    protected override async Task LoadPageAsync()
    {
        var result = (await mediator.Send(new GetRecipeListQuery(Filter, PagingOptions)));
        if (result.IsFailure)
        {
            return;
        }

        Recipes.Clear();
        foreach (var item in result.Value.Items)
        {
            Recipes.Add(item);
        }

        TotalItemsCount = result.Value.TotalItemsCount;
        TotalPages = (int)Math.Ceiling((double)result.Value.TotalItemsCount / PagingOptions.PageSize);
        UpdatePageNumbers();
    }
    
    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
        => await navigationService.GoToAsync(NavigationService.RecipeDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(RecipeDetailViewModel.Id)] = new RecipeId(id)
            }
        );

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync(NavigationService.RecipeEditRouteRelative);
    }

    public void Receive(RecipeEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(RecipeDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}
