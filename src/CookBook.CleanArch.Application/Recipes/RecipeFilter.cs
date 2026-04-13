using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Domain.Recipes.Enums;

namespace CookBook.CleanArch.Application.Recipes;

public class RecipeFilter : IFilter<RecipeSortParameter>
{
    public string? Name { get; set; }
    public RecipeType? RecipeType { get; set; }
    public TimeSpan? MinimalDuration { get; set; }
    public TimeSpan? MaximalDuration { get; set; }
    public RecipeSortParameter SortParameter { get; set; }
    public bool IsSortAscending { get; set; } = true;
}

public enum RecipeSortParameter
{
    Name,
    Type,
    Duration,
    CreatedAt,
    ModifiedAt
}
