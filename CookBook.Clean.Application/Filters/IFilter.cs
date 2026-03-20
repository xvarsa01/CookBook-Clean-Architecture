namespace CookBook.Clean.Application.Filters;

public interface IFilter
{
    string? SortParameterName { get; set; }
    bool IsSortAscending { get; set; }
}
