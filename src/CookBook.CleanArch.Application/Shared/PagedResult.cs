namespace CookBook.CleanArch.Application.Shared;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalItemsCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
