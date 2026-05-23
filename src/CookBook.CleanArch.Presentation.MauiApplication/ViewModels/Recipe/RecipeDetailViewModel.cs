using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CookBook.CleanArch.Application.Recipes.Commands;
using CookBook.CleanArch.Application.Recipes.Models;
using CookBook.CleanArch.Application.Recipes.Queries;
using CookBook.CleanArch.Domain.Ingredients.ValueObjects;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.Errors;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Presentation.MauiApplication.Messages;
using CookBook.CleanArch.Presentation.MauiApplication.Resources.Texts;
using CookBook.CleanArch.Presentation.MauiApplication.Services;
using CookBook.CleanArch.Presentation.MauiApplication.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.ObjectModel;
using CookBook.CleanArch.Presentation.MauiApplication.Extensions;

namespace CookBook.CleanArch.Presentation.MauiApplication.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class  RecipeDetailViewModel(
    IMediator mediator,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RecipeEditMessage>, IRecipient<RecipeIngredientAddMessage>,
        IRecipient<RecipeIngredientDeleteMessage>
{
    private readonly RecipeReviewListModel.Validator _reviewValidator = new();
    
    [ObservableProperty]
    public partial RecipeId Id { get; set; } = null!;

    [ObservableProperty]
    public partial RecipeResponse? Recipe { get; set; }

    [ObservableProperty]
    public partial decimal? AverageMark { get; set; }
    
    [ObservableProperty]
    public partial ObservableCollection<RecipeReviewResponse> Reviews { get; set; } = [];
    
    [ObservableProperty]
    public partial RecipeReviewListModel ReviewNew { get; set; } = RecipeReviewListModel.Empty;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        var result = (await mediator.Send(new GetRecipeDetailQuery(Id)));
        if (result.IsSuccess)
        {
            Recipe = result.Value;
            Reviews = [.. result.Value.Reviews];
            AverageMark = result.Value.AverageMark;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Recipe is not null)
        {
            var result = (await mediator.Send(new DeleteRecipeCommand(Id)));
            if (result.IsSuccess)
            {
                MessengerService.Send(new RecipeDeleteMessage());

                navigationService.SendBackButtonPressed();
            }
        }
    }
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Recipe is not null)
        {
            await navigationService.GoToAsync(NavigationService.RecipeEditRouteRelative,
                new Dictionary<string, object?>
                {
                    [nameof(RecipeEditViewModel.Id)] = Id
                }
            );
        }
    }

    [RelayCommand]
    private async Task AddReviewAsync()
    {
        if (Recipe is null)
        {
            return;
        }

        if (!await ValidateNewReviewAsync())
            return;

        var request = new RecipeAddReviewRequest(ReviewNew.Mark, ReviewNew.Description);
        var result = await mediator.Send(new AddReviewToRecipeCommand(Id, request));
        if (result.IsFailure)
            return;

        var createdReview = new RecipeReviewResponse(result.Value, request.Mark, request.Description);
        Reviews.Insert(0, createdReview);
        RecalculateAverageMark();
        ReviewNew = RecipeReviewListModel.Empty;
        ForceDataRefreshOnNextAppearing();
    }

    [RelayCommand]
    private async Task RemoveReviewAsync(RecipeReviewResponse review)
    {
        if (Recipe is null)
            return;

        var result = await mediator.Send(new RemoveReviewFromRecipeCommand(Id, review.Id));
        if (result.IsFailure)
            return;

        Reviews.Remove(review);
        RecalculateAverageMark();
        ForceDataRefreshOnNextAppearing();
    }

    private void RecalculateAverageMark()
    {
        if (Recipe is null)
            return;

        AverageMark = Reviews.Count == 0
            ? null
            : Reviews.Average(review => (decimal)review.Mark);
    }
    
    [RelayCommand]
    private async Task GoToIngredientDetailAsync(IngredientId id)
    {
        ForceDataRefreshOnNextAppearing();
        
        await navigationService.GoToAsync(
            $"{NavigationService.IngredientListRouteAbsolute}{NavigationService.IngredientDetailRouteRelative}",
            new Dictionary<string, object?> { [nameof(IngredientDetailViewModel.Id)] = id }
        );
    }
    
    [RelayCommand]
    private async Task<bool> ValidateNewReviewAsync()
    {
        ReviewNew.ValidationResults = await _reviewValidator.ValidateAsync(ReviewNew);
        
        OnPropertyChanged(nameof(ReviewNew.ValidationResults));
        
        return ReviewNew.ValidationResults.IsValid;
    }

    public void Receive(RecipeEditMessage message)
    {
        if (message.RecipeId == Id)
        {
            ForceDataRefreshOnNextAppearing();
        }
    }

    async partial void OnIdChanged(RecipeId value)
    {
        try
        {
            await LoadDataAsync();
        }
        catch (Exception)
        {
            // ignored
        }
    }
    
    public void Receive(RecipeIngredientAddMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }

    public void Receive(RecipeIngredientDeleteMessage message)
    {
        ForceDataRefreshOnNextAppearing();
    }
}

public partial class RecipeReviewListModel : ObservableObject
{
    [ObservableProperty]
    public required partial int Mark { get; set; }
    
    [ObservableProperty]
    public required partial string Description { get; set; }
    
    [ObservableProperty]
    public partial ValidationResult? ValidationResults {get; set; } = new();

    public static RecipeReviewListModel Empty         
        => new()
        {
            Mark = 5,
            Description = string.Empty
        };
    
    public static string ReviewMarkProperty => nameof(Mark);
    public static string ReviewDescriptionProperty => nameof(Description);
    
    public class Validator : AbstractValidator<RecipeReviewListModel>
    {
        public Validator()
        {
            RuleFor(x => x.Mark)
                .InclusiveBetween(1, 5)
                .WithMessage(model => RecipeReviewErrors.RecipeReviewMarkOutOfRangeError(model.Mark).ToLocalizedMessage());
            
            RuleFor(x => x.Description)
                .MaximumLength(Recipe.MaxReviewDescriptionLength)
                .WithMessage(_ => RecipeReviewErrors.RecipeReviewDescriptionTooLongError().ToLocalizedMessage());
        }
    }
}
