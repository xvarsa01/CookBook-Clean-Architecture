using CookBook.CleanArch.Domain.RecipeRoot;
using CookBook.CleanArch.Domain.RecipeRoot.Enums;

namespace CookBook.CleanArch.Application.Filters;

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
