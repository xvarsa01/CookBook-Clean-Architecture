namespace CookBook.Clean.Application.Filters;

public class IngredientFilter : IFilter
{
    public string? Name { get; set; }
    public bool? HasDescription { get; set; }
    public bool? HasImage { get; set; }
    public string? SortParameterName { get; set; }
    public bool IsSortAscending { get; set; } =  true;
}
