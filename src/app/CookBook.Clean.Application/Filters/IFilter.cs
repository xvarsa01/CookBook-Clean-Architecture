namespace CookBook.Clean.Application.Filters;

public interface IFilter<TSortParameter >
{
    TSortParameter  SortParameter { get; set; }
    bool IsSortAscending { get; set; }
}
