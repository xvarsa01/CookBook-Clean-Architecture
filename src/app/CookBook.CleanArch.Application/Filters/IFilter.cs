namespace CookBook.CleanArch.Application.Filters;

public interface IFilter<TSortParameter >
{
    TSortParameter  SortParameter { get; set; }
    bool IsSortAscending { get; set; }
}
