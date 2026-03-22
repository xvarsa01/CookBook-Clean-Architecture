using CookBook.Clean.Core.RecipeRoot;
using CookBook.Clean.Core.RecipeRoot.Enums;

namespace CookBook.Clean.Application.Filters;

public class RecipeFilter : IFilter
{
    public string? Name { get; set; }
    public RecipeType? RecipeType { get; set; }
    public TimeSpan? MinimalDuration { get; set; }
    public TimeSpan? MaximalDuration { get; set; }
    public string? SortParameterName { get; set; }
    public bool IsSortAscending { get; set; } = true;
}
