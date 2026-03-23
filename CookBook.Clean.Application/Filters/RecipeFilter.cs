using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.Application.Filters;

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
