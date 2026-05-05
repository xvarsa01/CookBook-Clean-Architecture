using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Recipes;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using MediatR;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

public partial class RecipeListViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelWithPager<RecipeFilter, RecipeSortParameter>(messengerService), IRecipient<RecipeEditMessage>, IRecipient<RecipeDeleteMessage>, IRecipient<LanguageChangedMessage>
{
    [ObservableProperty]
    public partial ObservableCollection<RecipeListResponse> Recipes { get; set; } = [];

    public override RecipeFilter Filter { get; set; } = new();

    public bool UseMinimalDuration
    {
        get => Filter.MinimalDuration.HasValue;
        set
        {
            if (value == Filter.MinimalDuration.HasValue)
                return;

            if (value)
            {
                Filter.MinimalDuration = MinimalDuration;
            }
            else
            {
                Filter.MinimalDuration = null;
            }

            OnPropertyChanged();
            OnPropertyChanged(nameof(MinimalDuration));
        }
    }
    
    public bool UseMaximalDuration
    {
        get => Filter.MaximalDuration.HasValue;
        set
        {
            if (value == Filter.MaximalDuration.HasValue)
                return;

            if (value)
            {
                Filter.MaximalDuration = MaximalDuration;
            }
            else
            {
                Filter.MaximalDuration = null;
            }

            OnPropertyChanged();
            OnPropertyChanged(nameof(MaximalDuration));
        }
    }

    public TimeSpan MinimalDuration
    {
        get => Filter.MinimalDuration ?? TimeSpan.Zero;
        set
        {
            Filter.MinimalDuration = UseMinimalDuration ? value : null;

            OnPropertyChanged();
            OnPropertyChanged(nameof(UseMinimalDuration));
        }
    }
    
    public TimeSpan MaximalDuration
    {
        get => Filter.MaximalDuration ?? TimeSpan.Zero;
        set
        {
            Filter.MaximalDuration = UseMaximalDuration ? value : null;

            OnPropertyChanged();
            OnPropertyChanged(nameof(UseMaximalDuration));
        }
    }

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
    private async Task GoToDetailAsync(RecipeId id)
        => await navigationService.GoToAsync(NavigationService.RecipeDetailRouteRelative,
            new Dictionary<string, object?>
            {
                [nameof(RecipeDetailViewModel.Id)] = id
            }
        );

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync(NavigationService.RecipeCreateRouteRelative);
    }

    public void Receive(RecipeEditMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(RecipeDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
    
    public void Receive(LanguageChangedMessage message)
    {
        RefreshRecipeTypes();
    }
    
    private void RefreshRecipeTypes()
    {
        var items = Recipes.ToList();
        Recipes = new ObservableCollection<RecipeListResponse>(items);      // workaround to trigger UI update of recipe types after language change,
    }
}
