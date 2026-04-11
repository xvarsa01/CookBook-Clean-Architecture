namespace CookBook.CleanArch.Application.Abstraction;

public interface IFilter<TSortParameter >
{
    TSortParameter  SortParameter { get; set; }
    bool IsSortAscending { get; set; }
}
