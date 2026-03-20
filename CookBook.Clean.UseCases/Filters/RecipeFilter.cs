using CookBook.Clean.Core.RecipeRoot;

namespace CookBook.Clean.UseCases.Filters;

public class RecipeFilter
{
    public string? Name { get; set; }
    public RecipeType? RecipeType { get; set; }
    public TimeSpan? MinimalDuration { get; set; }
    public TimeSpan? MaximalDuration { get; set; }
    public string? SortParameterName { get; set; }
    public bool IsSortAscending { get; set; } = true;
}
