namespace CookBook.CleanArch.Application.Filters;

public class IngredientFilter : IFilter<IngredientsSortParameter>
{
    public string? Name { get; set; }
    public bool? HasDescription { get; set; }
    public bool? HasImage { get; set; }
    public IngredientsSortParameter SortParameter { get; set; }
    public bool IsSortAscending { get; set; } =  true;
}

public enum IngredientsSortParameter
{
    Name,
}
